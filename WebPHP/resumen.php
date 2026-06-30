<?php

session_start();

if (!isset($_SESSION["documento"]))
{
    header("Location: ingreso.html");
    exit();
}

include("conexion.php");

$documento =
    $_SESSION["documento"];

$sql = "
SELECT
    l.periodo,
    l.fecha_vencimiento,
    l.total_a_pagar,
    l.pago_minimo,
    t.numero_tarjeta,
    t.banco_emisor
FROM usuarios u
JOIN tarjetas t
    ON u.documento = t.dni_titular
JOIN liquidaciones l
    ON t.num_cuenta = l.num_cuenta
WHERE u.documento = ?
";

$stmt = $conexion->prepare($sql);

$stmt->bind_param(
    "s",
    $documento
);

$stmt->execute();

$resultado =
    $stmt->get_result();

echo "<h1>Bienvenido "
    . $_SESSION["nombre"]
    . "</h1>";

while ($fila =
    $resultado->fetch_assoc())
{
    echo "<hr>";

    echo "Tarjeta: "
        . $fila["numero_tarjeta"]
        . "<br>";

    echo "Banco: "
        . $fila["banco_emisor"]
        . "<br>";

    echo "Periodo: "
        . $fila["periodo"]
        . "<br>";

    echo "Vencimiento: "
        . $fila["fecha_vencimiento"]
        . "<br>";

    echo "Total: $"
        . $fila["total_a_pagar"]
        . "<br>";

    echo "Pago mínimo: $"
        . $fila["pago_minimo"]
        . "<br>";
}
?>