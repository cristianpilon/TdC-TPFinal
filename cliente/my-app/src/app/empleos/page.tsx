"use client";
import Layout from "../layoutUser";

export default function Empleos(){
  const postulacionClick = () => alert('Se ha postulado correctamente a la posición. Se ha enviado un correo de confirmación.\nEn breve recibirá novedades respecto al progreso de su postulación.') 
  return (
  <Layout userLayout={true}>
    <h1 className="Titulo2">Implementadores de Software de Gestión Buenos Aires</h1> 
    <div className="cuerpo">
      <div className="etiquetas"> <span className="etiqueta">Analista funcional</span><span className="etiqueta">Diseño sistemas</span><span className="etiqueta">Atención cliente</span></div>
      <div className="contenedor"> 
          <div className="detalle">
              <div className="row">
                <div className="col-50"><label>Ubicación: </label>Rosario, Santa Fe</div>
                <div className="col-50"><label>Modalidad de trabajo: </label>Full-Time</div>
              </div>
              <div className="row">
                <div className="col-50"><label>Categoría: </label>Analista Funcional</div>
                <div className="col-50"><label>Publicado: </label>19/05/2023</div>
              </div>
              <div className="row">
                <div className="col-50"><label>Horario: </label>Lunes a Viernes de 8 a 16hs</div>
                <div className="col-50"><label>Tipo de trabajo: </label>Híbrido</div>
              </div>
          </div>

          <div className="boton-postulacion">
            <button onClick={postulacionClick} id="botonpost">POSTULARME</button>
          </div>
      </div>

      <div className="Descripcion"> 
        <h2>DESCRIPCION DEL EMPLEO</h2>
        <p>Implementadores de Software de Gestión para nuestra sucursal de Buenos Aires</p> 
        <p>Para sumarte al equipo estamos buscando personas:</p> 
        <ul>
          <li>Con experiencia en implementación y/o seguimiento de proyectos.</li>  
          <li>Con conocimientos en lenguajes de programación.</li>  
          <li>Software de gestión y/o procesos contables.</li>  
          <li>Con experiencia en trabajo en equipo para el cumplimiento de objetivos en tiempo y forma.</li>  
          <li>Con disponibilidad para viajar.</li>  
        </ul>
        <p>Imagnate:</p>
        <ul>
          <li>Liderando proyectos de implementación de múltiples clientes en simultaneo.</li>  
          <li>Relevando necesidades y requenmiontos de nuestros clientes.</li>  
          <li>Implementando, capacitando y poniendo en marcha los distintos circuitos.</li>  
          <li>Garantizando una atención de excelencia a nuestros clientes.</li>  
        </ul>
        <p>Modalidad:</p>
        <ul>
            <li>Full time.</li>  
            <li>Sucursal: Buenos aires</li>  
          </ul>
      </div>
    </div>
  </Layout>);
}