variable "digitalocean_token" {}
variable "spaces_access_id" {}
variable "spaces_private_key" {}
variable "dockerhub_username" {}


terraform {
    required_providers {
        digitalocean = {
            source = "digitalocean/digitalocean"
            version = "~> 2.8.0"
        }
        swarm = {
            source = "aucloud/swarm"
            version = "~> 1.2.2"
        }
        docker = {
            source = "kreuzwerker/docker"
            version = "~> 2.13.0"
        }
    }
}

provider "digitalocean" {
  token = var.digitalocean_token

  spaces_access_id  = var.spaces_access_id
  spaces_secret_key = var.spaces_private_key
}

resource "digitalocean_spaces_bucket" "terraform_state" {
  name   = "souffle/terraform.tfstate"
  region = "fra1"
}

resource "docker_image" "nginx" {
  name         = "nginx-souffle:latest"
  keep_locally = false
}

resource "docker_image" "app" {
    name         = "${var.dockerhub_username}/minitwit-souffle:latest"
    keep_locally = false
}

// manager node
resource "digitalocean_droplet" "manager" {
  name  = "manager"
  image = "docker-20-04"
  region = "fra1"
  size = "s-1vcpu-1gb"
  count = 1

  provisioner "remote-exec" {
    inline = [
      "docker swarm init --advertise-addr ${self.ipv4_address}",
      "docker swarm join-token worker -q > /tmp/join-token-worker",
      "docker swarm join-token manager -q > /tmp/join-token-manager",
    ]
  }
}

// worker nodes
resource "digitalocean_droplet" "worker" {
  name  = "worker"
  image = "docker-20-04"
  region = "fra1"
  size = "s-1vcpu-1gb"
  count = 2

  provisioner "remote-exec" {
    inline = [
      "docker swarm join --token ${file("/tmp/join-token-worker")} ${digitalocean_droplet.manager.ipv4_address}:8080"
    ]
  }
  ssh_keys = digitalocean.ssh_key.ids
}

resource "digitalocean_droplet" "nginx" {
  name  = "test"
  image = "docker-20-04"
  region = "fra1"
  size = "s-1vcpu-1gb"
  count = 3

  provisioner "remote-exec" {
    inline = [
      "docker swarm init --advertise-addr ${self.ipv4_address}",
      "docker swarm join-token worker -q > /tmp/join-token-worker",
      "docker swarm join-token manager -q > /tmp/join-token-manager",
    ]
  }
}
