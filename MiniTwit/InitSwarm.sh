#!/bin/bash

managerIp="<managerIp>"
workerIp1="<workerIp1>"
workerIp2="<workerIp2>"
loadBalancerIp="<loadBalancerIp>"
DOCKER_USERNAME="<DOCKER_USERNAME>"
CONNECTION_STRING="<CONNECTION_STRING>"
SERILOG_URI="<SERILOG_URI>"


managerToken=$(ssh """$managerIp""" "docker swarm init --advertise-addr ""$managerIp""")
ssh """$loadBalancerIp""" "docker swarm init --advertise-addr ""$loadBalancerIp"""
scp ".\nginx.conf" root@$loadBalancerIp:default.conf

ssh """$workerIp1""" "docker swarm join --token ""$managerToken"""
ssh """$workerIp2""" "docker swarm join --token ""$managerToken"""

# overlay network on manager node:
ssh """$managerIp""" "docker network create --driver overlay minitwit-net"

ssh """$managerIp""" "docker service create --name minitwit-souffle --publish 8080:80 --replicas 2 -e ""$CONNECTION_STRING"" -e ""$SERILOG_URI"" ""$DOCKER_USERNAME""/minitwit-souffle:latest"

ssh """$loadBalancerIp""" "docker service create --name minitwit-nginx --publish 80:80 --replicas 1 -v /root/default.conf:/etc/nginx/conf.d/default.conf nginx"
