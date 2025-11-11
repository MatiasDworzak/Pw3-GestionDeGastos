google.charts.load('current', { packages: ['corechart'] });
google.charts.setOnLoadCallback(drawChartInit);

// 🔹 Diccionario global para mantener colores consistentes
const colorPorCategoria = {};

// 🔹 Paleta de colores
const paletaColores = [
    "#6BAED6", "#FD8D3C", "#74C476", "#9E9AC8", "#FFA07A",
    "#FDD0A2", "#A1D99B", "#C6DBEF", "#E377C2", "#BCBD22"
];
let indiceColor = 0;

function obtenerColorCategoria(categoria) {
    if (!colorPorCategoria[categoria]) {
        colorPorCategoria[categoria] = paletaColores[indiceColor % paletaColores.length];
        indiceColor++;
    }
    return colorPorCategoria[categoria];
}

// --- 🔹 LISTENERS para evitar conflicto entre mes y rango ---
const filtroMesInput = document.getElementById("filtroMes");
const desdeInput = document.getElementById("desde");
const hastaInput = document.getElementById("hasta");

filtroMesInput.addEventListener("change", () => {
    if (filtroMesInput.value) {
        desdeInput.value = "";
        hastaInput.value = "";
    }
});

desdeInput.addEventListener("change", () => {
    if (desdeInput.value) {
        filtroMesInput.value = "";
    }
});

hastaInput.addEventListener("change", () => {
    if (hastaInput.value) {
        filtroMesInput.value = "";
    }
});

// --- 🔹 FUNCIÓN PRINCIPAL ---
async function drawChartInit() {

    const fechaActual = new Date();

    const params = new URLSearchParams(window.location.search);

    const mesActual = params.get("mes") || fechaActual.getMonth() + 1;
    const anioActual = params.get("anio") || fechaActual.getFullYear();

    const primerDia = new Date(anioActual, mesActual - 1, 1);
    const ultimoDia = new Date(anioActual, mesActual, 0);
    const nombreMes = primerDia.toLocaleString("es-ES", { month: "long" });

    document.getElementById("rango-fechas").textContent =
        `${nombreMes.charAt(0).toUpperCase() + nombreMes.slice(1)}: ${primerDia.toLocaleDateString()} al ${ultimoDia.toLocaleDateString()}`;

    const query = new URLSearchParams({ mes: `${anioActual}-${mesActual}` });
    const response = await fetch(`/Home/Filtrar?${query.toString()}`);
    const result = await response.json();

    console.log("📦 Datos recibidos:", result);

    drawPieChart(result.gastosDetallados);
    mostrarTopCategorias(result.top3);
    mostrarListaDeGastos(result.listaDeGastos || result.listaDeGastos); // tolerante a minúscula o mayúscula
}

// --- 🔹 Dibuja gráfico tipo dona ---
function drawPieChart(gastos) {
    if (!gastos || gastos.length === 0) {
        document.getElementById('chart_div').innerHTML = '<p class="text-muted">No hay gastos para mostrar.</p>';
        return;
    }

    const dataArray = [['Categoría', 'Monto']];
    const colores = [];

    gastos.forEach(g => {
        dataArray.push([g.categoria, g.totalCategoria]);
        colores.push(g.color);
    });

    const data = google.visualization.arrayToDataTable(dataArray);

    const formatter = new google.visualization.NumberFormat({
        prefix: '$',
        decimalSymbol: ',',
        groupingSymbol: '.',
        fractionDigits: 2
    });

    formatter.format(data, 1);

    const options = {
        pieHole: 0.55,
        backgroundColor: 'transparent',
        chartArea: { width: '100%', height: '90%' },
        legend: { position: 'right', textStyle: { color: '#333', fontSize: 12 } },
        pieSliceText: 'value',
        pieSliceTextStyle: { fontSize: 9 },
        colors: colores
    };

    const chart = new google.visualization.PieChart(document.getElementById('chart_div'));
    chart.draw(data, options);

    document.getElementById("topCategorias").classList.remove("d-none");
}

// --- 🔹 Top categorías ---
function mostrarTopCategorias(top3) {
    const contenedor = document.getElementById("topCategoriasContenido");

    if (!top3 || top3.length === 0) {
        contenedor.innerHTML = '<small class="text-muted">No hay categorías para mostrar.</small>';
        return;
    }

    contenedor.innerHTML = top3.map(cat => `
    <div class="text-center mx-2">
        <div class="d-flex align-items-center justify-content-center rounded-circle mx-auto mb-1 shadow-sm"
             style="width:60px;height:60px;background:${cat.color};">
            <span class="material-icons" style="color:white; font-size:1.8rem;">
                ${cat.icono}
            </span>
        </div>
        
        <small>${cat.categoria} (${cat.total.toLocaleString('es-AR', { style: 'currency', currency: 'ARS' })})</small>
    </div>
`).join('');
}

// --- 🔹 Mostrar lista de gastos ---
function mostrarListaDeGastos(listaDeGastos) {
    const contenedorVerGastos = document.getElementById("gastos-list");

    if (!listaDeGastos || listaDeGastos.length === 0) {
        contenedorVerGastos.innerHTML = '<div class="text-center text-muted py-4">No hay gastos registrados</div>';
        return;
    }
    console.log(listaDeGastos);

    // Formatear monto como moneda
    const formatearMonto = (monto) => {
        return new Intl.NumberFormat('es-AR', {
            style: 'currency',
            currency: 'ARS',
            minimumFractionDigits: 2
        }).format(monto);
    };

    contenedorVerGastos.innerHTML = listaDeGastos.map(g => `
        <a href="/GastoEspecifico/GastoEspecifico/${g.idGasto}"
           class="expense-item">
            <div class="expense-item-content">
                <span class="material-icons expense-icon"
                      style="color:${g.color};">
                    ${g.icono}
                </span>
                <div class="expense-details">
                    <div class="expense-name">${g.nombre}</div>
                    <div class="expense-date">${new Date(g.fecha + "T00:00:00").toLocaleDateString('es-AR')}</div>
                </div>
            </div>
            <span class="expense-amount">
                ${formatearMonto(g.montoTotal)}
            </span>
        </a>
    `).join('');
}

// --- 🔹 Submit del filtro ---
document.getElementById("filtroFechasForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const mes = filtroMesInput.value;
    const desde = desdeInput.value;
    const hasta = hastaInput.value;

    const query = new URLSearchParams();

    if (desde && hasta) {
        query.set("desde", desde);
        query.set("hasta", hasta);
    } else if (mes) {
        query.set("mes", mes);
    }

    const response = await fetch(`/Home/Filtrar?${query.toString()}`);
    const result = await response.json();

    drawPieChart(result.gastosDetallados);
    mostrarTopCategorias(result.top3);
    mostrarListaDeGastos(result.listaDeGastos || result.listaDeGastos);

    if (desde && hasta) {
        document.getElementById("rango-fechas").textContent = `${desde} al ${hasta}`;
    } else if (mes) {
        const [anio, mesNum] = mes.split("-");
        const primerDia = new Date(anio, mesNum - 1, 1);
        const ultimoDia = new Date(anio, mesNum, 0);
        const nombreMes = primerDia.toLocaleString("es-ES", { month: "long" });
        document.getElementById("rango-fechas").textContent =
            `${nombreMes.charAt(0).toUpperCase() + nombreMes.slice(1)}: ${primerDia.toLocaleDateString()} al ${ultimoDia.toLocaleDateString()}`;
    }
});

// --- 🔹 Redibuja al cambiar tamaño ---
window.addEventListener("resize", () => {
    if (typeof drawChartInit === "function") {
        drawChartInit();
    }
});
