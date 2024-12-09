"use client";
import { useState, useEffect, Key } from "react";
import Link from "next/link";
import Image from "next/image";
import Layout from "../layoutUser";
import { mensajeErrorGeneral } from "@/constants";
import { fetchPrivado } from "@/componentes/compartido";
import imgCompania from "../../../public/compania.png";
import Modal from "@/componentes/compartido/modal";
import { useRouter } from "next/navigation";
import ModalReporte from "@/componentes/empleos/modalReporte";

export default function Empleos() {
  const { push } = useRouter();

  const [titulo, setTitulo] = useState<string>("");
  const [mensajeModal, setMensajeModal] = useState<string>();
  const [empleos, setEmpleos] = useState<
    Array<{
      id: string;
      titulo: string;
      fechaPublicacion: string;
      ubicacion: string;
      empresa: string;
      empresaLogo: string;
      destacado: boolean;
      nuevo: boolean;
      remuneracion: number;
      perfiles: Array<string>;
      etiquetas: Array<string>;
    }>
  >();

  const [empleosSugeridos, setEmpleosSugeridos] = useState<
    Array<{
      id: string;
      titulo: string;
      fechaPublicacion: string;
      ubicacion: string;
      empresa: string;
      empresaLogo: string;
      remuneracion: number;
      destacado: boolean;
      nuevo: boolean;
    }>
  >();

  const [mostrarModalReporte, setMostrarModalReporte] =
    useState<boolean>(false);

  useEffect(() => {
    cargarEmpleos();
    cargarEmpleosSugeridos();

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const cargarEmpleos = async () => {
    await fetchPrivado(
      `http://localhost:4000/empleos/${titulo ? `?criterio=${titulo}` : ""}`,
      "GET"
    )
      .then(async (data) => {
        if (data.ok) {
          const respuesta = await data.json();
          const empleos = respuesta.empleos as Array<{
            id: string;
            titulo: string;
            fechaPublicacion: string;
            ubicacion: string;
            empresa: string;
            empresaLogo: string;
            destacado: boolean;
            nuevo: boolean;
            remuneracion: number;
            perfiles: Array<{ id: string; nombre: string }>;
            etiquetas: Array<{ id: string; nombre: string }>;
          }>;
          const empleosMap = empleos.map<{
            id: string;
            titulo: string;
            fechaPublicacion: string;
            ubicacion: string;
            empresa: string;
            empresaLogo: string;
            destacado: boolean;
            nuevo: boolean;
            remuneracion: number;
            perfiles: Array<string>;
            etiquetas: Array<string>;
          }>(
            ({
              id,
              titulo,
              fechaPublicacion,
              ubicacion,
              empresa,
              empresaLogo,
              destacado,
              nuevo,
              remuneracion,
              perfiles,
              etiquetas,
            }) => ({
              id,
              titulo,
              fechaPublicacion,
              ubicacion,
              empresa,
              empresaLogo,
              destacado,
              nuevo,
              remuneracion,
              perfiles: perfiles.map((p) => p.nombre),
              etiquetas: etiquetas.map((p) => p.nombre),
            })
          );
          setEmpleos(empleosMap);
          return;
        } else if (data.status === 400) {
          const respuesta = await data.json();
          const validaciones = respuesta.Validaciones.map(
            (validacion: { Mensaje: string }) => validacion.Mensaje
          ).join("\r\n");

          setMensajeModal(validaciones);

          return;
        } else if (data.status === 401) {
          push("/");
          return;
        }

        setMensajeModal(mensajeErrorGeneral);
      })
      .catch(() => {
        setMensajeModal(mensajeErrorGeneral);
      });
  };

  const cargarEmpleosSugeridos = async () => {
    await fetchPrivado("http://localhost:4000/empleos/sugeridos", "GET")
      .then(async (data) => {
        if (data.ok) {
          const respuesta = await data.json();
          const empleos = respuesta.empleos as Array<{
            id: string;
            titulo: string;
            fechaPublicacion: string;
            ubicacion: string;
            empresa: string;
            empresaLogo: string;
            destacado: boolean;
            nuevo: boolean;
            remuneracion: number;
            perfiles: Array<{ id: string; nombre: string }>;
          }>;
          const empleosMap = empleos.map<{
            id: string;
            titulo: string;
            fechaPublicacion: string;
            ubicacion: string;
            empresa: string;
            empresaLogo: string;
            destacado: boolean;
            nuevo: boolean;
            remuneracion: number;
            perfiles: Array<string>;
          }>(
            ({
              id,
              titulo,
              fechaPublicacion,
              ubicacion,
              empresa,
              empresaLogo,
              destacado,
              nuevo,
              remuneracion,
              perfiles,
            }) => ({
              id,
              titulo,
              fechaPublicacion,
              ubicacion,
              empresa,
              empresaLogo,
              destacado,
              nuevo,
              remuneracion,
              perfiles: perfiles.map((p) => p.nombre),
            })
          );
          setEmpleosSugeridos(empleosMap);
          return;
        } else if (data.status === 400) {
          const respuesta = await data.json();
          const validaciones = respuesta.Validaciones.map(
            (validacion: { Mensaje: string }) => validacion.Mensaje
          ).join("\r\n");

          setMensajeModal(validaciones);

          return;
        } else if (data.status === 401) {
          push("/");
          return;
        }

        setMensajeModal(mensajeErrorGeneral);
      })
      .catch(() => {
        setMensajeModal(mensajeErrorGeneral);
      });
  };

  const limpiarModal = () => setMensajeModal(undefined);
  const clickBuscar = () => {
    setEmpleos(undefined);
    cargarEmpleos();
  };
  const clickMostrarModalReporte = () => {
    setMostrarModalReporte(true);
  };

  const clickBusquedaAvanzada = () => {
    push("/empleos/busqueda-avanzada");
  };

  return (
    <Layout>
      <div className="cuerpo">
        <div className="botonera-empleos my-5 pb-4">
          <div className="relative h-9 basis-1/2 flex pr-4">
            <input
              type="text"
              id="searchBox"
              name="searchBox"
              className="border px-4 py-2 flex-1"
              placeholder="Buscar por título, empresa o ubicación"
              value={titulo}
              onChange={(e) => setTitulo(e.target.value)}
              onKeyUp={(e) => {
                if (e.key === "Enter") {
                  clickBuscar();
                }
              }}
            />
            <button
              className="ml-0 boton text-black font-bold py-2 px-4"
              onClick={clickBuscar}
            >
              Buscar
            </button>
          </div>
          <div className="flex relative h-9 basis-1/2 pl-4 justify-end">
            <button
              onClick={clickMostrarModalReporte}
              className="boton text-black font-bold py-2 px-4"
            >
              Reporte
              {/* Descargar listado */}
            </button>

            <button
              onClick={clickBusquedaAvanzada}
              className="boton btn-primary text-white font-bold py-2 px-4 ml-2"
            >
              Búsqueda avanzada
            </button>
          </div>
        </div>

        {empleos && (
          <div className="flex flex-wrap justify-start mt-4">
            <div
              id="busqueda-empleos-items"
              className="relative basis-1/2 pr-4"
            >
              <h1 className="font-bold">Empleos</h1>
              {empleos.map((empleo: any, index: Key) => (
                <div
                  key={index}
                  className="entrada-empleo flex flex-row p-2 uai-shadow mt-4"
                >
                  <div className="entrada-empleo-logo pr-1">
                    <Image
                      priority={true}
                      width={150}
                      height={150}
                      src={
                        empleo.empresaLogo
                          ? `data:image/jpeg;base64,${empleo.empresaLogo}`
                          : imgCompania
                      }
                      fill={false}
                      alt=""
                    />
                  </div>
                  <div className="entrada-empleo-descripcion pl-1 flex flex-grow">
                    <div className="flex flex-1 flex-col">
                      <div className="entrada-empleo-detalle">
                        <div className="flex flex-wrap flex-col">
                          <div className="ml-auto text-xs mb-1">
                            <label
                              className={`bg-red-500 px-1 font-bold text-white uppercase text-xs ${
                                empleo.destacado ? "" : "invisible"
                              }`}
                            >
                              Destacado
                            </label>
                            <label
                              className={`bg-green-500 px-1 font-bold text-white ml-1 uppercase text-xs ${
                                empleo.nuevo ? "" : "hidden"
                              }`}
                            >
                              Nuevo
                            </label>
                          </div>
                          <div>
                            <h3 className="font-bold">
                              <Link
                                href={`/empleos/[id]`}
                                as={`/empleos/${empleo.id}`}
                              >
                                {empleo.titulo}
                              </Link>
                            </h3>
                          </div>
                        </div>
                        <div className="flex text-xs mt-2">
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 24 24"
                            strokeWidth="1.5"
                            stroke="currentColor"
                            className="w-4 h-4 text-blue-800 mr-1"
                          >
                            <path
                              strokeLinecap="round"
                              strokeLinejoin="round"
                              d="M15 10.5a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z"
                            />
                            <path
                              strokeLinecap="round"
                              strokeLinejoin="round"
                              d="M19.5 10.5c0 7.142-7.5 11.25-7.5 11.25S4.5 17.642 4.5 10.5a7.5 7.5 0 1 1 15 0Z"
                            />
                          </svg>
                          {empleo.ubicacion}
                        </div>
                        <div className="flex text-xs">
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 24 24"
                            strokeWidth="1.5"
                            stroke="currentColor"
                            className="w-4 h-4 text-blue-800 mr-1"
                          >
                            <path
                              strokeLinecap="round"
                              strokeLinejoin="round"
                              d="M3.75 21h16.5M4.5 3h15M5.25 3v18m13.5-18v18M9 6.75h1.5m-1.5 3h1.5m-1.5 3h1.5m3-6H15m-1.5 3H15m-1.5 3H15M9 21v-3.375c0-.621.504-1.125 1.125-1.125h3.75c.621 0 1.125.504 1.125 1.125V21"
                            />
                          </svg>
                          {empleo.empresa}
                        </div>
                        <div className="flex text-xs">
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 24 24"
                            strokeWidth="1.5"
                            stroke="currentColor"
                            className="w-4 h-4 text-blue-800 mr-1"
                          >
                            <path
                              strokeLinecap="round"
                              strokeLinejoin="round"
                              d="M12 6v12m-3-2.818.879.659c1.171.879 3.07.879 4.242 0 1.172-.879 1.172-2.303 0-3.182C13.536 12.219 12.768 12 12 12c-.725 0-1.45-.22-2.003-.659-1.106-.879-1.106-2.303 0-3.182s2.9-.879 4.006 0l.415.33M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z"
                            />
                          </svg>
                          {empleo.remuneracion.toLocaleString("en-US", {
                            style: "currency",
                            currency: "USD",
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 0,
                          })}
                        </div>
                      </div>
                      <div className="entrada-empleo-botones mt-auto pt-2 flex justify-end">
                        <button className="text-xs boton text-black font-bold py-2 px-4">
                          Guardar oferta
                        </button>
                        <button className="ml-2 boton text-black font-bold py-2 px-4">
                          Ver oferta
                        </button>
                        <button className="boton btn-primary font-bold ml-2 py-2 px-4">
                          Postularme
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>
            <div
              id="busqueda-empleos-sugeridos"
              className="relative basis-1/2 pl-4"
            >
              <h1 className="font-bold">Postulaciones sugeridas</h1>
              {empleosSugeridos &&
                empleosSugeridos.map((empleo: any, index: Key) => (
                  <div
                    key={index}
                    className="entrada-empleo flex flex-row p-2 uai-shadow mt-4"
                  >
                    <div className="entrada-empleo-logo pr-1">
                      <Image
                        priority={true}
                        width={150}
                        height={150}
                        src={
                          empleo.empresaLogo
                            ? `data:image/jpeg;base64,${empleo.empresaLogo}`
                            : imgCompania
                        }
                        fill={false}
                        alt=""
                      />
                    </div>
                    <div className="entrada-empleo-descripcion pl-1 flex flex-grow">
                      <div className="flex flex-1 flex-col">
                        <div className="entrada-empleo-detalle">
                          <div className="flex flex-wrap flex-col">
                            <div className="ml-auto text-xs mb-1">
                              <label
                                className={`bg-red-500 px-1 font-bold text-white uppercase text-xs ${
                                  empleo.destacado ? "" : "invisible"
                                }`}
                              >
                                Destacado
                              </label>
                              <label
                                className={`bg-green-500 px-1 font-bold text-white ml-1 uppercase text-xs ${
                                  empleo.nuevo ? "" : "hidden"
                                }`}
                              >
                                Nuevo
                              </label>
                            </div>
                            <div>
                              <h3 className="font-bold">
                                <Link
                                  href={`/empleos/[id]`}
                                  as={`/empleos/${empleo.id}`}
                                >
                                  {empleo.titulo}
                                </Link>
                              </h3>
                            </div>
                          </div>
                          <div className="flex text-xs mt-2">
                            <svg
                              xmlns="http://www.w3.org/2000/svg"
                              fill="none"
                              viewBox="0 0 24 24"
                              strokeWidth="1.5"
                              stroke="currentColor"
                              className="w-4 h-4 text-blue-800 mr-1"
                            >
                              <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                d="M15 10.5a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z"
                              />
                              <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                d="M19.5 10.5c0 7.142-7.5 11.25-7.5 11.25S4.5 17.642 4.5 10.5a7.5 7.5 0 1 1 15 0Z"
                              />
                            </svg>
                            {empleo.ubicacion}
                          </div>
                          <div className="flex text-xs">
                            <svg
                              xmlns="http://www.w3.org/2000/svg"
                              fill="none"
                              viewBox="0 0 24 24"
                              strokeWidth="1.5"
                              stroke="currentColor"
                              className="w-4 h-4 text-blue-800 mr-1"
                            >
                              <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                d="M3.75 21h16.5M4.5 3h15M5.25 3v18m13.5-18v18M9 6.75h1.5m-1.5 3h1.5m-1.5 3h1.5m3-6H15m-1.5 3H15m-1.5 3H15M9 21v-3.375c0-.621.504-1.125 1.125-1.125h3.75c.621 0 1.125.504 1.125 1.125V21"
                              />
                            </svg>
                            {empleo.empresa}
                          </div>
                          <div className="flex text-xs">
                            <svg
                              xmlns="http://www.w3.org/2000/svg"
                              fill="none"
                              viewBox="0 0 24 24"
                              strokeWidth="1.5"
                              stroke="currentColor"
                              className="w-4 h-4 text-blue-800 mr-1"
                            >
                              <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                d="M12 6v12m-3-2.818.879.659c1.171.879 3.07.879 4.242 0 1.172-.879 1.172-2.303 0-3.182C13.536 12.219 12.768 12 12 12c-.725 0-1.45-.22-2.003-.659-1.106-.879-1.106-2.303 0-3.182s2.9-.879 4.006 0l.415.33M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z"
                              />
                            </svg>
                            {empleo.remuneracion.toLocaleString("en-US", {
                              style: "currency",
                              currency: "USD",
                              minimumFractionDigits: 0,
                              maximumFractionDigits: 0,
                            })}
                          </div>
                        </div>
                        <div className="entrada-empleo-botones mt-auto pt-2 flex justify-end">
                          <button className="text-xs boton text-black font-bold py-2 px-4">
                            Guardar oferta
                          </button>
                          <button className="ml-2 boton text-black font-bold py-2 px-4">
                            Ver oferta
                          </button>
                          <button className="boton btn-primary font-bold ml-2 py-2 px-4">
                            Posularme
                          </button>
                        </div>
                      </div>
                    </div>
                  </div>
                ))}
            </div>
          </div>
        )}
      </div>
      {mensajeModal && (
        <Modal
          mostrar={!!mensajeModal}
          titulo={"Error"}
          onCambiarModal={limpiarModal}
        >
          <p>{mensajeModal}</p>
        </Modal>
      )}
      {empleos && (
        <ModalReporte
          empleos={empleos}
          mostrar={mostrarModalReporte}
          onCambiarModal={setMostrarModalReporte}
        ></ModalReporte>
      )}
    </Layout>
  );
}
