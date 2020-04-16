output "reader_dns_name" {
  value       = aws_instance.easyrates-reader.public_dns
  description = "Reader dns name"
}

output "writer_dns_name" {
  value       = aws_instance.easyrates-writer.public_dns
  description = "Writer dns name"
}

output "db_dns_name" {
  value       = aws_instance.easyrates-db.public_dns
  description = "Db dns name"
}

output "tank_dns_name" {
  value       = aws_instance.easyrates-tank.public_dns
  description = "Tank dns name"
}