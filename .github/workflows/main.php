<?php
$connect = mysqli_connect("localhost", "root", "", "db_seu_ecommerce_2026");

$select_from_roles = "SELECT * FROM roles";

$result_of_roles = mysqli_query($connect, $select_from_roles);
$row_of_roles = mysqli_fetch_all($result_of_roles);
?>