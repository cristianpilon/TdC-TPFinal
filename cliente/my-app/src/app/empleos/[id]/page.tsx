"use client";
import { useRouter } from 'next/navigation';
import { useEffect, useState } from "react";
import Layout from "../../layoutUser";
import { mensajeErrorGeneral } from "@/constants";
import { fetchPrivado, obtenerTokenSesion } from "@/componentes/compartido";

export default function Empleo({ params }: { params: { id: string } }){
  const { push } = useRouter();
  
  const [empleo, setEmpleo] = useState<{ 
    id:string, 
    titulo: string, 
    descripcion: string,
    fechaPublicacion: string, 
    ubicacion: string, 
    remuneracion: number, 
    modalidadTrabajo:string,
    horariosLaborales: string,
    tipoTrabajo: string,  
    perfiles: Array<{id:number, nombre:string}>, 
    etiquetas: Array<{id:number, nombre:string}>
  }>();

  useEffect(() => {
    const fetchData = async () => {
      await fetch(`http://localhost:4000/empleos/${params.id}`, { 
        method: 'GET',
        headers: {
          "Content-Type": "application/json",
        },
      }).then(async (data) => {
        if (data.ok) {
          const respuesta = await data.json();
          setEmpleo(respuesta.empleo);
          return;
        }
        
        alert(mensajeErrorGeneral);
      }).catch((error) => {
        alert(error);
      });

    };

    fetchData();
  }, [params.id])

  const postulacionClick = async () => {
    await fetchPrivado(`http://localhost:4000/postulaciones/${params.id}`, 'POST')
      .then(async (data) => {
      if (data.ok) {
        alert('Se ha postulado correctamente a la posición. Se ha enviado un correo de confirmación.\nEn breve recibirá novedades respecto al progreso de su postulación.');
        return;
      }
      else if (data.status === 400) {
        const respuesta = await data.json();
        respuesta.Validaciones.forEach((validacion: any) => {
          alert(validacion.Mensaje);
        });
        
        return;
      }

      alert(mensajeErrorGeneral);
    }).catch((error) => {
      alert(error);
    });
  }

  const backButtonClick = async () => {
    push('/empleos');
  }
   
  return (
  <Layout userLayout={true}>
    <div className="cuerpo">
    {empleo && (
      <><h1 className="Titulo2">{empleo.titulo}</h1> 
      
        <div className="etiquetas">
          {empleo.etiquetas.map((e, index) => (<span key={index} className="etiqueta">{e.nombre}</span>))}
        </div>
        <div className="contenedor"> 
            <div className="detalle">
                <div className="row">
                  <div className="col-50"><label>Ubicación: </label>{empleo.ubicacion}</div>
                  <div className="col-50"><label>Modalidad de trabajo: </label>{empleo.modalidadTrabajo}</div>
                </div>
                <div className="row">
                  <div className="col-50"><label>Categoría: </label>{empleo.perfiles.map(p => p.nombre).join(" / ")}</div>
                  <div className="col-50"><label>Publicado: </label>{empleo.fechaPublicacion}</div>
                </div>
                <div className="row">
                  <div className="col-50"><label>Horario: </label>{empleo.horariosLaborales}</div>
                  <div className="col-50"><label>Tipo de trabajo: </label>{empleo.tipoTrabajo}</div>
                </div>
            </div>
            <div className="boton-postulacion">
              <button onClick={postulacionClick} id="botonpost">POSTULARME</button>
            </div>
        </div>
  
        <div className="Descripcion" dangerouslySetInnerHTML={{__html: empleo.descripcion}}>
        </div>
      <button onClick={backButtonClick} type="button" className="boton-volver text-white bg-red-700 hover:bg-red-800 focus:outline-none focus:ring-4 focus:ring-red-300 font-medium rounded-full text-sm px-5 py-2.5 text-center me-2 mb-2 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900">Volver</button>   
      </>
    )}
    {!empleo && (
      <>
        <div className="spinner" role="status">
            <svg aria-hidden="true" className="w-8 h-8 text-gray-200 animate-spin dark:text-gray-600 fill-blue-600" viewBox="0 0 100 101" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z" fill="currentColor"/>
                <path d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z" fill="currentFill"/>
            </svg>
            <span className="sr-only">Cargando...</span>
        </div>
        <button onClick={backButtonClick} type="button" className="boton-volver text-white bg-red-700 hover:bg-red-800 focus:outline-none focus:ring-4 focus:ring-red-300 font-medium rounded-full text-sm px-5 py-2.5 text-center me-2 mb-2 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900">Volver</button>   
      </>
    )}
    </div>


  </Layout>);
}