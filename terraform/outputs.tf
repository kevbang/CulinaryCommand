output "instance_name" {
  value = aws_lightsail_instance.culinary_command_app.name
}

output "public_ip" {
  value = aws_lightsail_static_ip.culinary_command_app.ip_address
}

output "ssh_example" {
  value = "ssh ubuntu@${aws_lightsail_static_ip.culinary_command_app.ip_address}"
}