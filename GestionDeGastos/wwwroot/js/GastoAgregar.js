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

// ======== EVENT LISTENERS ========

// Radios con secciones
radiosConSection.forEach(r => r.addEventListener('change', e => mostrarSeccion(e.target.id, { bloquearMonto: true })));

// Radio sin ticket
radioSinTicket.addEventListener('change', e => {
    sections.forEach(s => s.classList.add("d-none"));
    if (e.target.checked) {
        limpiarListaDeItems();
        document.getElementById("MontoTotal").readOnly = false; 
    }
});

// Radio ticket con foto
document.getElementById("ticket-foto").addEventListener('change', e => {
    if (e.target.checked) limpiarListaDeItems();
});

// Agregar nuevo item
btnAgregarItem.addEventListener('click', () => {
    const items = Array.from(document.querySelectorAll(".item-data-container"));
    const ultimo = items.at(-1);
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
