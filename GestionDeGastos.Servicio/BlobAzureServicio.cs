using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeGastos.Servicio
{
    public interface IBlobAzureServicio
    {
        string EliminarBlob(string nombre, string nombreContenedor);
        BlobContainerClient obtenerContenedor(string nombreContenedor);

        Task<string> SubirBlobAsync(IFormFile archivo, string nombreContenedor);
    }

    public class BlobAzureServicio : IBlobAzureServicio
    {

        private readonly BlobServiceClient _clienteBlob;

        public BlobAzureServicio(IConfiguration configuracion)
        {
            string keys = configuracion["Blob:ConnectionString"];
            _clienteBlob = new BlobServiceClient(keys);
        }
        public string EliminarBlob(string nombre, string nombreContenedor)
        {
            BlobContainerClient contenedor = obtenerContenedor(nombreContenedor);
            BlobClient clienteBlob = contenedor.GetBlobClient(nombre);
            clienteBlob.DeleteIfExists();
            return nombre;
        }

        public BlobContainerClient obtenerContenedor(string nombreContenedor)
        {
            BlobContainerClient contenedor = _clienteBlob.GetBlobContainerClient(nombreContenedor);
            return contenedor;
        }

        public async Task<string> SubirBlobAsync(IFormFile archivo, string nombreContenedor)
        {
            Stream stream = archivo.OpenReadStream();
            string nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
            BlobContainerClient contenedor = obtenerContenedor(nombreContenedor);
            BlobClient clienteBlob = contenedor.GetBlobClient(nombreArchivo);
            await clienteBlob.UploadAsync(stream);

            return nombreArchivo;
        }
    }
}
