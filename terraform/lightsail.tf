resource "aws_lightsail_instance" "culinary_command_app" {
    name              = var.lightsail_instance_name
    availability_zone = "us-east-2a"
    blueprint_id      = var.blueprint_id
    bundle_id         = var.bundle_id

    tags = {
        component = "culinarycommand"
    }
}

resource "aws_lightsail_static_ip" "culinary_command_app" {
    name = "${var.lightsail_instance_name}-ip"
}

resource "aws_lightsail_static_ip_attachment" "culinary_command_app" {
    static_ip_name = aws_lightsail_static_ip.culinary_command_app.name
    instance_name  = aws_lightsail_instance.culinary_command_app.name
}

resource "aws_lightsail_instance_public_ports" "culinary_command_app" {
    instance_name = aws_lightsail_instance.culinary_command_app.name


    # Allow public access to specific ports
    # SSH
    port_info {
        protocol  = "tcp"
        from_port = 22
        to_port   = 22
    } 

    # HTTP
    port_info {
        protocol  = "tcp"
        from_port = 80
        to_port   = 80
    }

    # HTTPS
    port_info {
        protocol  = "tcp"
        from_port = 443
        to_port   = 443
    }


}