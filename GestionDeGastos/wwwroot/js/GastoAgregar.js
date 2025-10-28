//// Logica secciones que abren los radio button

//const montoInicialContainer = document.querySelector("#monto-inical-container");
//const fechaContainer = document.querySelector("#fecha-container");

//const sections = Array.from(document.querySelectorAll("section"));

//const radiosConSection = document.querySelectorAll(".radio-con-section");

//radiosConSection.forEach((r) => {
//    r.addEventListener('change', (e) => {
//        sections.forEach(s => s.classList.add("d-none"));

//        const radioCambiado = e.target;
//        const sectionDelRadioCambiado = document.querySelector(`#${radioCambiado.id}-section`);
//        sectionDelRadioCambiado.classList.remove("d-none");

//        document.getElementById("MontoTotal").value = '';

//        //montoInicialContainer.classList.add("d-none");
//        //fechaContainer.classList.replace('col-6', 'col-12');
//    });
//});


//// logica de radio gasto sin ticket

//const radioSinTicket = document.getElementById("sin-ticket");

//radioSinTicket.addEventListener('change', (e) => {
//    sections.forEach(s => s.classList.add("d-none"));
//    if (e.target.checked) {
//        //montoInicialContainer.classList.remove("d-none");
//        //fechaContainer.classList.replace('col-12', 'col-6');

//        LimpiarListaDeItems();
//        document.getElementById("MontoTotal").value='';
//    }
//});

//const radioTicketConFoto = document.getElementById("ticket-foto");

//radioTicketConFoto.addEventListener('change', (e) => { if (e.target.checked) LimpiarListaDeItems(); } );



//function LimpiarListaDeItems() {
//    Array.from(document.querySelectorAll(".item-data-container")).forEach((item, index) => {
//        if (index === 0) item.querySelectorAll("input").forEach(i => i.value = '');
//        else item.remove();
//    });
//    actualizarAtributosDeInputsDeLosItems();
//}


//// logica de nuevos items en ticket manual

//const btnAgregarNuevoItem = document.getElementById("nuevo-item");

//btnAgregarNuevoItem.addEventListener('click', () => {
//    const form = document.getElementById("form-agregar-gasto");
//    const itemsSelector = ".item-data-container";

//    const ultimoItem = Array.from(document.querySelectorAll(itemsSelector)).at(-1);
//    const nuevoItem = ultimoItem.cloneNode(true);

//    // 1) Limpiar valores del clone
//    nuevoItem.querySelectorAll("input").forEach(i => {
//        if (i.type !== "hidden") {
//            i.value = "";
//            i.classList.remove("input-validation-error");
//            i.removeAttribute("aria-invalid");
//            i.removeAttribute("aria-describedby");
//        }
//    });

//    // 2) Limpiar mensajes de validación clonados (¡NO ELIMINAR LOS SPANS!)
//    nuevoItem.querySelectorAll("span[data-valmsg-for]").forEach(span => {
//        // Limpiamos el texto del error
//        span.textContent = "";
//        // Lo reseteamos a su estado "válido" (sin error)
//        span.classList.remove("field-validation-error");
//        span.classList.add("field-validation-valid");
//    });

//    // 3) Insertar el clone en el DOM
//    ultimoItem.after(nuevoItem);

//    // 4) Reindexar TODOS los items (Esto es correcto)
//    actualizarAtributosDeInputsDeLosItems();

//    // 5) Resetear la metadata de validación del form y volver a parsear (Esto es correcto)
//    const $form = $(form);
//    $form.removeData("validator");
//    $form.removeData("unobtrusiveValidation");

//    $.validator.unobtrusive.parse(form);
//});

//// logica borrar item y modal del ultimo item

//const modalHTML = document.getElementById('modal-eliminar-ultimo-item');

//document.addEventListener("click", function (e) { // lo hago a nivel documento porque tengo que contemplar los botones que aun no existen
//    if (e.target.classList.contains("eliminar-item")) {
//        if (document.querySelectorAll('.eliminar-item').length > 1) {
//            e.target.closest(".item-data-container").remove();
//            actualizarAtributosDeInputsDeLosItems(); // modificara los names de los inputs que ya teniamos para que el viewmodel los procese bien.
//            calcularMontoTotal();
//        } else {
//            const modal = new bootstrap.Modal(modalHTML);
//            modal.show();
//        }
//    }

//    if (e.target.id === 'btn-modal-sin-ticket') {
//        radioSinTicket.click();
//        const modal = bootstrap.Modal.getInstance(modalHTML);
//        modal.hide();
//    }
//});


//function actualizarAtributosDeInputsDeLosItems() {
//    const items = Array.from(document.querySelectorAll(".item-data-container"));
//    items.forEach((item, index) => {
//        const inputsOriginales = item.querySelectorAll(".input-original-item");


//        inputsOriginales[0].setAttribute("name", `Items[${index}].Descripcion`);
//        inputsOriginales[0].setAttribute("id", `Items_${index}__Descripcion`);
//        inputsOriginales[0].parentElement.querySelector("span").setAttribute("data-valmsg-for", `Items[${index}].Descripcion`);
//        inputsOriginales[0].parentElement.querySelector('input[name$="__Invariant"]')?.setAttribute("value", `Items[${index}].Descripcion`);


//        inputsOriginales[1].setAttribute("name", `Items[${index}].Cantidad`);
//        inputsOriginales[1].setAttribute("id", `Items_${index}__Cantidad`);
//        inputsOriginales[1].parentElement.querySelector("span").setAttribute("data-valmsg-for", `Items[${index}].Cantidad`);
//        inputsOriginales[1].parentElement.querySelector('input[name$="__Invariant"]')?.setAttribute("value", `Items[${index}].Cantidad`);

//        inputsOriginales[2].setAttribute("name", `Items[${index}].PrecioUnitario`);
//        inputsOriginales[2].setAttribute("id", `Items_${index}__PrecioUnitario`);
//        inputsOriginales[2].parentElement.querySelector("span").setAttribute("data-valmsg-for", `Items[${index}].PrecioUnitario`);
//        inputsOriginales[2].parentElement.querySelector('input[name$="__Invariant"]')?.setAttribute("value", `Items[${index}].PrecioUnitario`);
//    });
//}

//// sumatoria de items para reflejar el monto total dinamicamente(se ejecuta tanto cuando se borra un item, o cambia los valores de algun input del item)
////, igualmente tambien se validara en el backend

//function calcularMontoTotal() {

//    const inputMontoTotal = document.getElementById("MontoTotal");
//    let total = 0;

//    const items = Array.from(document.querySelectorAll(".item-data-container"));
//    items.forEach((item, index) => {
//        const inputsOriginales = item.querySelectorAll(".input-original-item");

//        let cantidadDelItem = parseFloat(inputsOriginales[1].value) || 0;
//        let precioUnitario = parseFloat(inputsOriginales[2].value) || 0;

//        total += cantidadDelItem * precioUnitario;
//    });

//    inputMontoTotal.value = total;
//}

//document.addEventListener("input", e => {
//    if (e.target.matches("input[name$='Cantidad'], input[name$='PrecioUnitario']")) {
//        calcularMontoTotal();
//    }
//});

//// codigo para que se cargue la seccion seleccionada en la anterior peticion
//window.addEventListener('DOMContentLoaded', () => {
//    const radioSeleccionado = document.querySelector('input[name="OpcionTicketSeleccionada"]:checked');
//    if (radioSeleccionado) {
//        radioSeleccionado.dispatchEvent(new Event('change'));
//    }
//});

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
