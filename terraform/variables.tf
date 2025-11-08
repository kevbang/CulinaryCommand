variable "lightsail_instance_name" {
  type    = string
  default = "culinary-command"
}

variable "blueprint_id" {
  type    = string
  default = "ubuntu_22_04"
}

variable "bundle_id" {
  type    = string
  default = "nano_2_0"
}

variable "key_pair_name" {
    type = string
    default = "culinary-command-key"
}