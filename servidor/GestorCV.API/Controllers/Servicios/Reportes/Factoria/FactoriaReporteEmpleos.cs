using ChartJSCore.Models;
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

        private void AgregarGraficoTorta(Document documento, Dictionary<string, float> valores)
        {
            //// Crea los datos del gráfico
            //var data = new Dictionary<string, int>
            //{
            //    {"A", 10},
            //    {"B", 20},
            //    {"C", 30},
            //};

            //// Crea el gráfico
            //var chart = new Chart();
            //chart.Type = Enums.ChartType.Pie;

            //chart.Data = new PieDataset
            //{
                
            //}


            //// Exporta el gráfico a una imagen
            //var imageBytes = await chart.ToImage();

            //// Guarda la imagen en el disco
            //var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "chart.png");
            //using (var fileStream = new FileStream(imagePath, FileMode.Create))
            //{
            //    fileStream.Write(imageBytes, 0, imageBytes.Length);
            //}


            //var chart = new Chart { Width = 300, Height = 450, RenderType = RenderType.ImageTag, AntiAliasing = AntiAliasingStyles.All, TextAntiAliasingQuality = TextAntiAliasingQuality.High };

            //// Crear un PlotModel con un PieSeries
            //PieChart pieChart = new PieChart();

            //var model = new PlotModel { Title = "Gráfico Circular" };
            //var series = new PieSeries();
            //series.Slices.Add(new PieSlice("Categoría A", 30));
            //series.Slices.Add(new PieSlice("Categoría B", 40));
            //series.Slices.Add(new PieSlice("Categoría C", 20));
            //series.Slices.Add(new PieSlice("Categoría D", 10));
            //model.Series.Add(series);
            //iTextSharp.text.charts;
            //// Convertir el PlotModel a una imagen
            //var ancho = 600;
            //var alto = 400;
            //var pngExporter = new PngExporter { Width = 600, Height = 400, Background = OxyColors.White };
            //pngExporter.Export(plotModel, stream);
            //var bitmap = new OxyPlot..WindowsForms.PngExporter().ExportToBitmap(model, ancho, alto, OxyColor.FromRgb(255, 255, 255));

            //// Guardar la imagen en un MemoryStream
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            //    // Crear un documento PDF con iTextSharp
            //    using (var pdfStream = new FileStream("GraficoCircular.pdf", FileMode.Create))
            //    {
            //        PdfWriter writer = new PdfWriter(pdfStream);
            //        PdfDocument pdf = new PdfDocument(writer);
            //        Document document = new Document(pdf);

            //        // Agregar la imagen al documento PDF
            //    }

            //    iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Jpeg);

            //    var img = new iTextSharp.text.Image(iText.Kernel.Pdf.ImageDataFactory.Create(ms.ToArray()));
            //        documento.Add(img);
            //}

        }
    }
}
