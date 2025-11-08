// ======== CONSTANTES / SELECTORES ========
const montoContainer = document.querySelector("#monto-inical-container");
const fechaContainer = document.querySelector("#fecha-container");
const sections = Array.from(document.querySelectorAll("section"));
const radiosConSection = document.querySelectorAll(".radio-con-section");
const radioSinTicket = document.getElementById("sin-ticket");
const btnAgregarItem = document.getElementById("nuevo-item");
const modalHTML = document.getElementById('modal-eliminar-ultimo-item');
const formGasto = document.getElementById("form-agregar-gasto");


// ======== FUNCIONES ========

// Mostrar u ocultar secciones según radio
function mostrarSeccion(radioId, { limpiarMonto = true, bloquearMonto = false } = {}) {
    sections.forEach(s => s.classList.add("d-none"));
    const seccion = document.querySelector(`#${radioId}-section`);
    if (seccion) seccion.classList.remove("d-none");

    if (limpiarMonto) document.getElementById("MontoTotal").value = '';
    if (bloquearMonto) document.getElementById("MontoTotal").readOnly = true; 
}

// Limpiar un item individual
function limpiarItem(item) {
    item.querySelectorAll("input").forEach(i => {
        if (i.type !== "hidden") {
            i.value = "";
            i.classList.remove("input-validation-error");
            i.removeAttribute("aria-invalid");
            i.removeAttribute("aria-describedby");
        }
    });

    item.querySelectorAll("span[data-valmsg-for]").forEach(span => {
        span.textContent = "";
        span.classList.remove("field-validation-error");
        span.classList.add("field-validation-valid");
    });
}

// Limpiar todos los items
function limpiarListaDeItems() {
    Array.from(document.querySelectorAll(".item-data-container")).forEach((item, index) => {
        if (index === 0) limpiarItem(item);
        else item.remove();
    });
    reindexarItems();
}

// Actualizar atributos de todos los items para que el binding funcione
function reindexarItems() {
    const items = Array.from(document.querySelectorAll(".item-data-container"));
    items.forEach((item, index) => {
        const inputs = item.querySelectorAll(".input-original-item");

        ["Descripcion", "Cantidad", "PrecioUnitario"].forEach((prop, i) => {
            const input = inputs[i];
            input.setAttribute("name", `Items[${index}].${prop}`);
            input.setAttribute("id", `Items_${index}__${prop}`);
            const span = input.parentElement.querySelector("span");
            if (span) span.setAttribute("data-valmsg-for", `Items[${index}].${prop}`);
            input.parentElement.querySelector('input[name$="__Invariant"]')?.setAttribute("value", `Items[${index}].${prop}`);
        });
    });
}

// Calcular monto total
function calcularMontoTotal() {
    let total = 0;
    Array.from(document.querySelectorAll(".item-data-container")).forEach(item => {
        const inputs = item.querySelectorAll(".input-original-item");
        const cantidad = parseFloat(inputs[1].value) || 0;
        const precio = parseFloat(inputs[2].value) || 0;
        total += cantidad * precio;
    });
    document.getElementById("MontoTotal").value = total;
}

function obtenerUltimoItem() {
    const items = Array.from(document.querySelectorAll(".item-data-container"));
    return items.at(-1);
} 

function limpiarArchivoTicket() {
    document.getElementById("formFile").value = "";
    document.getElementById("preview-ticket").src = "";
    document.getElementById("columna-items").classList.replace("col-9", "col-12");
}

// ======== EVENT LISTENERS ========

// Radios con secciones
radiosConSection.forEach(r => r.addEventListener('change', e => mostrarSeccion(e.target.id, { bloquearMonto: true })));

// Radio sin ticket
radioSinTicket.addEventListener('change', e => {
    sections.forEach(s => s.classList.add("d-none"));
    if (e.target.checked) {
        limpiarListaDeItems();
        limpiarArchivoTicket();
    }
});

// Radio ticket con foto
document.getElementById("ticket-foto").addEventListener('change', e => {
    if (e.target.checked) limpiarListaDeItems();
});

// Radio ticket manual
document.getElementById("ticket-manual").addEventListener('change', e => {
    if (e.target.checked) {
        document.getElementById("aviso-ticket-escaneado").classList.add("d-none");
        document.getElementById("titulo-ticket-manual-section").classList.remove("d-none");

        limpiarArchivoTicket();
    }
});

// Agregar nuevo item
btnAgregarItem.addEventListener('click', () => {
    const ultimo = obtenerUltimoItem();
    const clone = ultimo.cloneNode(true);
    limpiarItem(clone);
    ultimo.after(clone);
    reindexarItems();
    $(formGasto).removeData("validator").removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse(formGasto);
});

// Borrar item
document.addEventListener("click", e => {
    if (e.target.classList.contains("eliminar-item")) {
        const allItems = document.querySelectorAll('.eliminar-item');
        if (allItems.length > 1) {
            e.target.closest(".item-data-container").remove();
            reindexarItems();
            calcularMontoTotal();
        } else new bootstrap.Modal(modalHTML).show();
    }
    if (e.target.id === 'btn-modal-sin-ticket') {
        radioSinTicket.click();
        bootstrap.Modal.getInstance(modalHTML)?.hide();
    }
});

// Calcular monto al modificar inputs
document.addEventListener("input", e => {
    if (e.target.matches("input[name$='Cantidad'], input[name$='PrecioUnitario']")) calcularMontoTotal();
});

// Inicializar sección según radio marcado
window.addEventListener('DOMContentLoaded', () => {
    const seleccionado = document.querySelector('input[name="OpcionTicketSeleccionada"]:checked');
    //if (seleccionado) seleccionado.dispatchEvent(new Event('change'));
    if (Array.from(radiosConSection).includes(seleccionado)) mostrarSeccion(seleccionado.id, { limpiarMonto: false, bloquearMonto: true });
    else if (radioSinTicket.checked) limpiarListaDeItems();
});

// ======== SECCION DE ESCANEO DE TICKET ========
const btnEscanear = document.getElementById("btn-escanear-ticket"); 
const inputFile = document.getElementById("formFile"); 

btnEscanear.addEventListener('click', async (e) => {

    const file = inputFile.files[0];
    if (!file) {
        alert("Por favor, selecciona un archivo primero.");
        return;
    }

    // Mostrar un spinner/loading
    btnEscanear.disabled = true;
    btnEscanear.innerText = "Escaneando...";

    const formData = new FormData();
    formData.append("ticketFoto", file); // "ticketFoto" debe coincidir con el parámetro del Action

    try {
        const response = await fetch('/Gasto/EscanearTicket', {
            method: 'POST',
            body: formData
        });

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message || "Error al escanear");
        }

        const data = await response.json(); // data = TicketEscaneado

        // agrego imagen
        const preview = document.getElementById("preview-ticket");
        preview.src = URL.createObjectURL(file);
        preview.classList.remove("d-none");

        // ajusto columnas
        document.getElementById("columna-items").classList.replace("col-12", "col-9");

        // --- ¡AQUÍ POBLAMOS EL FORMULARIO! ---

        // Poblar la Fecha
        if (data.fechaEscaneada) {
            // Formatear fecha para el input type="date" (YYYY-MM-DD)
            const fechaParts = data.fechaEscaneada.split('-'); // Asumiendo que .NET la manda como YYYY-MM-DD
            document.getElementById("Fecha").value = `${fechaParts[0]}-${fechaParts[1]}-${fechaParts[2]}`;
        }

        // Poblar los items
        limpiarListaDeItems();

        console.log(data);

        data.itemsEscaneados.forEach((item, index) => {
            if (index > 0) {
                // Si hay más de un item, clonamos el anterior
                btnAgregarItem.click(); 
            }

            // Rellenar el item (el último que se haya agregado)
            const ultimoItem = obtenerUltimoItem();

            if (ultimoItem) {
                ultimoItem.querySelector("input[name$='.Descripcion']").value = item.descripcion;
                ultimoItem.querySelector("input[name$='.Cantidad']").value = item.cantidad;
                ultimoItem.querySelector("input[name$='.PrecioUnitario']").value = item.precioUnitario.toFixed(2);
            }
        });

        if (data.descuento) {
            btnAgregarItem.click(); 
            const ultimoItem = obtenerUltimoItem();
            if (ultimoItem) {
                ultimoItem.querySelector("input[name$='.Descripcion']").value = "Descuento";
                ultimoItem.querySelector("input[name$='.Cantidad']").value = 1;
                ultimoItem.querySelector("input[name$='.PrecioUnitario']").value = data.descuento;
            }
        }

        if (data.iva) {
            btnAgregarItem.click();
            const ultimoItem = obtenerUltimoItem();
            if (ultimoItem) {
                ultimoItem.querySelector("input[name$='.Descripcion']").value = "Impuesto";
                ultimoItem.querySelector("input[name$='.Cantidad']").value = 1;
                ultimoItem.querySelector("input[name$='.PrecioUnitario']").value = data.iva;
            }
        }

        calcularMontoTotal();

        // aca necesitaria hacer la logica para que se muestren solo los items sin el titulo de la seccion
        document.getElementById("ticket-manual-section").classList.remove("d-none");
        document.getElementById("aviso-ticket-escaneado").classList.remove("d-none");
        document.getElementById("titulo-ticket-manual-section").classList.add("d-none");

    } catch (error) {
        alert(error.message);
    } finally {
        btnEscanear.disabled = false;
        btnEscanear.innerText = "Escanear Ticket";
    }
});
