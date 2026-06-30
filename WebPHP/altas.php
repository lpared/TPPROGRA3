<?php

include("conexion.php");

$tipo_doc = $_POST["tipo_doc"];
$documento = $_POST["documento"];
$nombre = $_POST["nombre"];
$apellido = $_POST["apellido"];
$fecha = $_POST["fecha_nacimiento"];
$email = $_POST["email"];
$usuario = $_POST["usuario"];
$passwordA = $_POST["passwordA"];
$passwordB = $_POST["passwordB"];

if ($passwordA != $passwordB)
{
    die("Las contraseñas no coinciden.");
}

$sql = "
SELECT *
FROM usuarios
WHERE documento = ?
AND tipo_doc = ?
";

$stmt = $conexion->prepare($sql);

$stmt->bind_param(
    "ss",
    $documento,
    $tipo_doc
);

$stmt->execute();

$resultado = $stmt->get_result();

if ($resultado->num_rows == 0)
{
    die("El cliente no existe en el sistema.");
}

$sqlUpdate = "
UPDATE usuarios
SET usuario = ?,
    password = ?
WHERE documento = ?
";

$stmt2 = $conexion->prepare($sqlUpdate);

$stmt2->bind_param(
    "sss",
    $usuario,
    $passwordA,
    $documento
);

$stmt2->execute();

echo "Usuario web activado correctamente.";
?>