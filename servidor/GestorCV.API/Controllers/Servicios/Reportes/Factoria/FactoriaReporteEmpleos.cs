using ImageChartsLib;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
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
            title.Font = FontFactory.GetFont(FontFactory.TIMES, 18f, new BaseColor(42, 79, 134));
            title.Add("Listado de empleos");

            doc.Add(title);

            // Agregamos un parrafo vacio como separacion.
            doc.Add(new Paragraph(" "));

            // Empezamos a crear la tabla, definimos una tabla de 6 columnas
            var table = new PdfPTable(5);

            // Escribo encabezado de columnas
            table.AddCell(new PdfPCell(new Phrase("Título")) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Fecha")) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Ubicación")) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Etiquetas")) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Perfiles")) { BackgroundColor = BaseColor.LIGHT_GRAY });

            foreach (var empleo in empleos)
            {
                // Agrego filas
                table.AddCell(empleo.Titulo);
                table.AddCell(empleo.FechaPublicacion);
                table.AddCell(empleo.Ubicacion);
                var etiquetas = empleo.Etiquetas.Select(x => x.Nombre).ToList();
                table.AddCell(string.Join("/", etiquetas));
                var perfiles = empleo.Perfiles.Select(x => x.Nombre).ToList();
                table.AddCell(string.Join("/", perfiles));
            }

            // Agregamos la tabla al documento
            doc.Add(table);

            // Agregamos contador de empleos
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph($"Total de empleos: {empleos.Count}"));
            doc.Add(new Paragraph(" "));

            // Agrego gráficos de etiquetas y perfiles 
            AgregarGrafico(doc, empleos);

            // Landscape
            doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

            // Ceramos el documento
            doc.Close();

            return nombreArchivo;
        }

        private static void AgregarGrafico(Document doc, List<Models.Dtos.Empleo> empleos)
        {
            doc.Add(new Paragraph(" "));

            var diccionarioEtiquetas = new Dictionary<string, int>();
            var diccionarioPerfiles = new Dictionary<string, int>();

            foreach (var empleo in empleos)
            {
                foreach (var etiqueta in empleo.Etiquetas)
                {
                    if (diccionarioEtiquetas.ContainsKey(etiqueta.Nombre))
                    {
                        diccionarioEtiquetas[etiqueta.Nombre]++;
                        continue;
                    }

                    diccionarioEtiquetas.Add(etiqueta.Nombre, 1);
                }

                foreach (var perfil in empleo.Perfiles)
                {
                    if (diccionarioPerfiles.ContainsKey(perfil.Nombre))
                    {
                        diccionarioPerfiles[perfil.Nombre]++;
                        continue;
                    }

                    diccionarioPerfiles.Add(perfil.Nombre, 1);
                }
            }

            var graficoEtiquetas = new ImageCharts()
                .cht("bvs")
                .chs("500x190")
                .chd($"t:{string.Join(',', diccionarioEtiquetas.Values.Select(x => x.ToString()).ToArray())}")
                .chl(string.Join('|', diccionarioEtiquetas.Keys.Select(x => x).ToArray()))
                .chlps("font.size,12")
                .chxt("y")
                .chtt("Etiquetas");

            var base64Image = graficoEtiquetas.toDataURI().Replace("data:image/png;base64,", "");
            var imageBytes = Convert.FromBase64String(base64Image);
            var image = Image.GetInstance(imageBytes);

            doc.Add(image);

            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph(" "));

            var graficoPerfiles = new ImageCharts()
                .cht("bvs")
                .chs("500x190")
                .chd($"t:{string.Join(',', diccionarioPerfiles.Values.Select(x => x.ToString()).ToArray())}")
                .chl(string.Join('|', diccionarioPerfiles.Keys.Select(x => x).ToArray()))
                .chlps("font.size,12")
                .chxt("y")
                .chtt("Perfiles");

            base64Image = graficoPerfiles.toDataURI().Replace("data:image/png;base64,", "");
            imageBytes = Convert.FromBase64String(base64Image);
            image = Image.GetInstance(imageBytes);

            doc.Add(image);
        }
    }
}
