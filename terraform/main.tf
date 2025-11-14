terraform {
  required_version = ">= 1.6.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 6.20"
    }
  }

  backend "s3" {
    bucket  = "culinary-command-s3-bucket"
    key     = "terraform/culinary-command-terraform.tfstate"
    region  = "us-east-2"
    encrypt = true
  }
}

provider "aws" {
  region = "us-east-2"
}