#!/bin/bash

managerIp="<managerIp>"
workerIp1="<workerIp1>"
workerIp2="<workerIp2>"
loadBalancerIp="<loadBalancerIp>"
DOCKER_USERNAME="<DOCKER_USERNAME>"
CONNECTION_STRING="<CONNECTION_STRING>"
SERILOG_URI="<SERILOG_URI>"

Green='\033[0;32m'
NC='\033[0m' # No Color

echo -e "${Green}Init swarm on managers${NC}"
managerToken=$(ssh """root@$managerIp""" "docker swarm init --advertise-addr ""$managerIp""")
ssh """root@$loadBalancerIp""" "docker swarm init --advertise-addr ""$loadBalancerIp"""

echo -e "${Green}Copy nginx.conf to load balancer${NC}"
scp ".\nginx.conf" root@$loadBalancerIp:default.conf

echo -e "${Green}Join workers to swarm${NC}"
ssh """root@$workerIp1""" "docker swarm join --token ""$managerToken"""
# ssh """$workerIp2""" "docker swarm join --token ""$managerToken"""

# overlay network on manager node:
echo -e "${Green}Create overlay network on manager node${NC}"
ssh """root@$managerIp""" "docker network create --driver overlay minitwit-net"

# create services
echo -e "${Green}Create services${NC}"
ssh """root@$managerIp""" "docker service create --name minitwit-souffle --publish 8080:80 --replicas 2 -e ""$CONNECTION_STRING"" -e ""$SERILOG_URI"" ""$DOCKER_USERNAME""/minitwit-souffle:latest"

echo -e "${Green}Create nginx service${NC}"
ssh """root@$loadBalancerIp""" "docker service create --name minitwit-nginx --publish 80:80 --replicas 1 --mount type=bind,source=/root/default.conf,target=/etc/nginx/conf.d/default.conf,readonly nginx"
