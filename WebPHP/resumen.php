<?php

session_start();

if (!isset($_SESSION["documento"]))
{
    header("Location: ingreso.html");
    exit();
}

include("conexion.php");

$documento = $_SESSION["documento"];

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
ORDER BY l.periodo DESC
";

$stmt = $conexion->prepare($sql);
$stmt->bind_param("s", $documento);
$stmt->execute();

$resultado = $stmt->get_result();
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Resumen de Cuenta</title>
</head>
<body>

    <h1>
        Bienvenido, <?php echo $_SESSION["nombre"]; ?>
    </h1>

    <h3>
        Documento: <?php echo $_SESSION["documento"]; ?>
    </h3>

    <?php
    $filaActual = $resultado->fetch_assoc();

    if ($filaActual)
    {
    ?>

        <h2>Liquidación Actual</h2>

        <div
            style="
                border:1px solid gray;
                width:500px;
                padding:20px;
                margin-bottom:40px;
            "
        >
            <p>
                <strong>Tarjeta:</strong>
                <?php echo $filaActual["numero_tarjeta"]; ?>
            </p>

            <p>
                <strong>Banco:</strong>
                <?php echo $filaActual["banco_emisor"]; ?>
            </p>

            <p>
                <strong>Período:</strong>
                <?php echo $filaActual["periodo"]; ?>
            </p>

            <p>
                <strong>Fecha de vencimiento:</strong>
                <?php echo $filaActual["fecha_vencimiento"]; ?>
            </p>

            <p>
                <strong>Total a pagar:</strong>
                $ <?php echo $filaActual["total_a_pagar"]; ?>
            </p>

            <p>
                <strong>Pago mínimo:</strong>
                $ <?php echo $filaActual["pago_minimo"]; ?>
            </p>
        </div>

    <?php
    }
    else
    {
        echo "<h2>No posee liquidaciones registradas.</h2>";
    }
    ?>

    <h2>Historial de Liquidaciones</h2>

    <table border="1" cellpadding="10">
        <tr>
            <th>Período</th>
            <th>Fecha de vencimiento</th>
            <th>Total a pagar</th>
            <th>Pago mínimo</th>
        </tr>

        <?php
        while ($fila = $resultado->fetch_assoc())
        {
        ?>
            <tr>
                <td><?php echo $fila["periodo"]; ?></td>
                <td><?php echo $fila["fecha_vencimiento"]; ?></td>
                <td>$ <?php echo $fila["total_a_pagar"]; ?></td>
                <td>$ <?php echo $fila["pago_minimo"]; ?></td>
            </tr>
        <?php
        }
        ?>
    </table>

    <br><br>

    <a href="logout.php">
        Cerrar sesión
    </a>

</body>
</html>