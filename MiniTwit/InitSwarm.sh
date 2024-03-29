#!/bin/bash

managerIp="134.209.254.143"
workerIp1="165.227.161.26"
# workerIp2="<workerIp2>"
loadBalancerIp="164.92.200.109"
DOCKER_USERNAME="subjective0"
CONNECTION_STRING='"ConnectionStrings:MiniTwit-db"="Server=174.138.101.165;Database=MiniTwit-db;User Id=sa;Password=a282f131-5323-419c-9c8e-55535d486550;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True"'
SERILOG_URI="http://174.138.101.165:9090/"

Green='\033[0;32m'
NC='\033[0m' # No Color

echo -e "${Green}Init swarm on managers${NC}"
managerToken=$(ssh """root@$managerIp""" "docker swarm init --advertise-addr ""$managerIp""")
ssh """root@$loadBalancerIp""" "docker swarm init --advertise-addr ""$loadBalancerIp"""

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
ssh """root@$loadBalancerIp""" "docker service create --name nginx-souffle --publish 80:80 --replicas 1 ""$DOCKER_USERNAME""/nginx-souffle:latest"
