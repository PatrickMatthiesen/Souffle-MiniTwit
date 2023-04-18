#!/bin/bash

managerIp="<managerIp>"
workerIp1="<workerIp1>"
workerIp2="<workerIp2>"
DOCKER_USERNAME="<DOCKER_USERNAME>"

managerToken=$(ssh """$managerIp""" "docker swarm init --advertise-addr ""$managerIp""")

ssh """$workerIp1""" "docker swarm join --token ""$managerToken"""
ssh """$workerIp2""" "docker swarm join --token ""$managerToken"""

# overlay network on manager node with ssh:
ssh """$managerIp""" "docker network create --driver overlay minitwit-net"

ssh """$managerIp""" "docker service create --name minitwit-souffle --publish 80 --replicas 2 ""$DOCKER_USERNAME""/minitwit-souffle:latest"
