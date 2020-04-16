terraform {
  required_version = ">= 0.12, < 0.13"
}

provider "aws" {
  region = "eu-north-1"

  # Allow any 2.x version of the AWS provider
  version = "~> 2.0"
}

resource "aws_instance" "easyrates-reader" {
  ami                    = "ami-0b2e6fbf8c9364ab6"
  instance_type          = "t3.micro"
  availability_zone      = "eu-north-1a"
  vpc_security_group_ids = [aws_security_group.easyrates-common-sg.id]
  
  tags = {
    Name = "easyrates-reader"
  }
}

resource "aws_instance" "easyrates-writer" {
  ami                    = "ami-0b2e6fbf8c9364ab6"
  instance_type          = "t3.micro"
  availability_zone      = "eu-north-1a"
  vpc_security_group_ids = [aws_security_group.easyrates-common-sg.id]

  tags = {
    Name = "easyrates-writer"
  }
}

resource "aws_instance" "easyrates-db" {
  ami                    = "ami-0b2e6fbf8c9364ab6"
  instance_type          = "t3.micro"
  availability_zone      = "eu-north-1a"
  vpc_security_group_ids = [aws_security_group.easyrates-common-sg.id]

  tags = {
    Name = "easyrates-db"
  }
}

resource "aws_instance" "easyrates-tank" {
  ami                    = "ami-0b2e6fbf8c9364ab6"
  instance_type          = "t3.large"
  availability_zone      = "eu-north-1a"
  vpc_security_group_ids = [aws_security_group.easyrates-common-sg.id]

  tags = {
    Name = "easyrates-tank"
  }
}

resource "aws_security_group" "easyrates-common-sg" {
  name = "easyrates-common-sg"

  ingress {
    from_port   = 5010
    to_port     = 5010
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 5011
    to_port     = 5011
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 5432
    to_port     = 5432
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}