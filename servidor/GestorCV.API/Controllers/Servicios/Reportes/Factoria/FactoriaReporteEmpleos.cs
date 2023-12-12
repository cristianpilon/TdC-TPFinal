using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GestorCV.API.Controllers.Servicios.Reportes.Factoria
{
    /// <summary>
    /// Factoría de reporte para empleos.
    /// </summary>
    public class FactoriaReporteEmpleos
    {
        /// <summary>
        /// Genera un documento PDF como reporte en el servidor para listar los empleos disponibles en el sistema.
        /// </summary>
        /// <param name="empleos">Listado de empleos a incluir en el reporte.</param>
        /// <param name="usuario">Usuario que genera el reporte.</param>
        public static string Crear(List<Models.Dtos.Empleo> empleos, string usuario)
        {
            // Inicializamos el documento PDF
            var doc = new iTextSharp.text.Document();
            var nombreArchivo = $"{usuario} - Listado empleos.pdf";
            PdfWriter.GetInstance(doc, new FileStream(nombreArchivo, FileMode.Create));
            
            // Abrir el documento
            doc.Open();

            // Creamos un titulo personalizado con tamaño de fuente 18 y color Azul
            var title = new Paragraph();
            title.Font = FontFactory.GetFont(FontFactory.TIMES, 18f, new BaseColor(42,79, 134));
            title.Add("Listado de empleos");

            doc.Add(title);

            // Agregamos un parrafo vacio como separacion.
            doc.Add(new Paragraph(" "));

            // Empezamos a crear la tabla, definimos una tabla de 6 columnas
            var table = new PdfPTable(4);
            
            // Escribo encabezado de columnas
            table.AddCell(new PdfPCell(new Phrase("Título")) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Fecha")) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Ubicación")) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Perfiles")) { BackgroundColor = BaseColor.LIGHT_GRAY });

            foreach (var empleo in empleos)
            {
                // Agrego filas
                table.AddCell(empleo.Titulo);
                table.AddCell(empleo.FechaPublicacion);
                table.AddCell(empleo.Ubicacion);
                var perfiles = empleo.Perfiles.Select(x => x.Nombre).ToList();
                table.AddCell(string.Join("/", perfiles));
            }
            
            // Agregamos la tabla al documento
            doc.Add(table);
            // Ceramos el documento
            doc.Close();

            return nombreArchivo;
        }
    }
}
