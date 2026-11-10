window.klamDescargarDocumento = (contenido, nombre, tipo) => {
    const enlace = document.createElement('a');
    const url = URL.createObjectURL(new Blob([contenido], { type: tipo }));
    enlace.href = url;
    enlace.download = nombre;
    document.body.appendChild(enlace);
    enlace.click();
    enlace.remove();
    setTimeout(() => URL.revokeObjectURL(url), 1000);
};
