# KlamWA — Lab 9

Interfaz de BIOKLAM en C#, Blazor y Bootstrap. Esta entrega contiene la capa de presentación: utiliza datos ficticios y no se conecta con el backend Java, servicios web ni base de datos. Los formularios pueden validar entradas y mostrar avisos de demostración, pero no registran operaciones reales.

## Ejecutar

Requiere el SDK de .NET 10. Desde la raíz del repositorio ejecuta: dotnet run --project KlamWA/KlamWA.csproj. Abre la dirección local que indique la consola. En «Vista según rol» puedes recorrer las pantallas de administrador, vendedor, instrumentista y cliente.

## Recorrido sugerido

- **Administrador:** agenda, registro/edición/eliminación visual de cirugías, revisión de solicitudes, usuarios y RACI, plantillas de bandejas, despachos, consumos y reportes.
- **Vendedor:** formulario de solicitud con DNI y comprobante para paciente particular.
- **Cliente:** solicitudes, corrección visual de documento rechazado, calendario e historial de sus cirugías.
- **Instrumentista:** calendario y notificaciones de cirugías.

Las cirugías de ejemplo muestran etapas distintas del cierre: **CIR-001** tiene consumos despachados y usados; **CIR-006** espera orden de compra; **CIR-007** espera revisión de la orden; **CIR-008** espera registro de consumo. El detalle de una cirugía culminada incluye el comparativo de consumibles, con «Pendiente» cuando aún no se ha informado el consumo.

RF09 se presenta como **registro de cirugías por el administrador**, de acuerdo con la aclaración del equipo. La descarga Excel/PDF, el envío de correos, la autenticación y la persistencia quedan para la integración posterior con Java.
