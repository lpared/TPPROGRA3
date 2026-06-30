<?php

session_start();

include("conexion.php");

$tipo_doc = $_POST["tipo_doc"];
$documento = $_POST["documento"];
$usuario = $_POST["usuario"];
$password = $_POST["password"];

$sql = "
SELECT *
FROM usuarios
WHERE documento = ?
AND tipo_doc = ?
AND usuario = ?
AND password = ?
";

$stmt = $conexion->prepare($sql);

$stmt->bind_param(
    "ssss",
    $documento,
    $tipo_doc,
    $usuario,
    $password
);

$stmt->execute();

$resultado = $stmt->get_result();

if ($resultado->num_rows == 1)
{
    $fila = $resultado->fetch_assoc();

    $_SESSION["documento"] =
        $fila["documento"];

    $_SESSION["nombre"] =
        $fila["nombre"];

    header("Location: resumen.php");
    exit();
}
else
{
    echo "Usuario o contraseña incorrectos.";
}

?>