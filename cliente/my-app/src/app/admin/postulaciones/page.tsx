"use client";
import Link from "next/link";
import { useRouter } from "next/navigation";
import Layout from "../../layoutUser";
import Modal from "@/componentes/compartido/modal";
import { useEffect, useState } from "react";
import { fetchPrivado, formatearFecha } from "@/componentes/compartido";
import Spinner from "@/componentes/compartido/spinner";
import {
  EstadoDescartado,
  EstadoEntrevista,
  EstadoFinalizado,
  EstadoPendiente,
  EstadoRevisado,
  mensajeErrorGeneral,
} from "@/constants";

export default function Postulaciones() {
  const { push } = useRouter();
  const [mensajeModal, setMensajeModal] = useState<string>();
  const [tituloModal, setTituloModal] = useState<string>("");
  const [postulaciones, setPostulaciones] = useState<
    Array<{
      id: number;
      fecha: Date;
      empresa: string;
      titulo: string;
      estado: string;
      postulante: string;
    }>
  >();

  useEffect(() => {
    const obtenerPostulaciones = async () => {
      await fetchPrivado("http://localhost:4000/postulaciones/", "GET")
        .then(async (data) => {
          if (data.ok) {
            const respuesta: {
              postulaciones: {
                id: number;
                empleo: {
                  empresa: string;
                  titulo: string;
                  ubicacion: string;
                };
                usuario: {
                  nombre: string;
                  apellido: string;
                };
                estado: string;
                fecha: Date;
              }[];
            } = await data.json();

            const postulacionesFormateadas = respuesta.postulaciones.map(
              (x) => ({
                id: x.id,
                empresa: x.empleo.empresa,
                titulo: x.empleo.titulo,
                ubicacion: x.empleo.ubicacion,
                fecha: x.fecha,
                estado: x.estado,
                postulante: `${x.usuario.nombre} ${x.usuario.apellido}`,
              })
            );

            setPostulaciones(postulacionesFormateadas);
            return;
          }

          setTituloModal("Error");
          setMensajeModal(mensajeErrorGeneral);
        })
        .catch((error) => {
          setTituloModal("Error");
          setMensajeModal(error);
        });
    };

    obtenerPostulaciones();
  }, []);

  const backButtonClick = async () => {
    push("/admin");
  };

  const limpiarModal = () => setMensajeModal(undefined);

  return (
    <Layout>
      <div className="cuerpo">
        <div className="uai-shadow p-2 my-4">
          <h2 className="font-bold mb-2 inline">Postulaciones</h2>
          {postulaciones && (
            <div className="flex mt-2">
              <div className="flex items-center text-yellow-600 font-bold">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  strokeWidth={1.5}
                  stroke="currentColor"
                  className="w-4 h-4 mr-1"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M12 6v6h4.5m4.5 0a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z"
                  />
                </svg>
                {postulaciones!
                  .filter((x) => x.estado == EstadoPendiente)
                  .length.toString()}{" "}
                {EstadoPendiente}s
              </div>
              <div className="flex items-center text-blue-600 font-bold ml-2">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  strokeWidth={1.5}
                  stroke="currentColor"
                  className="w-4 h-4 mr-1"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M10.125 2.25h-4.5c-.621 0-1.125.504-1.125 1.125v17.25c0 .621.504 1.125 1.125 1.125h12.75c.621 0 1.125-.504 1.125-1.125v-9M10.125 2.25h.375a9 9 0 0 1 9 9v.375M10.125 2.25A3.375 3.375 0 0 1 13.5 5.625v1.5c0 .621.504 1.125 1.125 1.125h1.5a3.375 3.375 0 0 1 3.375 3.375M9 15l2.25 2.25L15 12"
                  />
                </svg>
                {postulaciones!
                  .filter((x) => x.estado == EstadoRevisado)
                  .length.toString()}{" "}
                {EstadoRevisado}s
              </div>
              <div className="flex items-center text-violet-600 font-bold ml-2">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  strokeWidth={1.5}
                  stroke="currentColor"
                  className="w-4 h-4 mr-1"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M8.625 9.75a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Zm0 0H8.25m4.125 0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Zm0 0H12m4.125 0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Zm0 0h-.375m-13.5 3.01c0 1.6 1.123 2.994 2.707 3.227 1.087.16 2.185.283 3.293.369V21l4.184-4.183a1.14 1.14 0 0 1 .778-.332 48.294 48.294 0 0 0 5.83-.498c1.585-.233 2.708-1.626 2.708-3.228V6.741c0-1.602-1.123-2.995-2.707-3.228A48.394 48.394 0 0 0 12 3c-2.392 0-4.744.175-7.043.513C3.373 3.746 2.25 5.14 2.25 6.741v6.018Z"
                  />
                </svg>
                {postulaciones!
                  .filter((x) => x.estado == EstadoEntrevista)
                  .length.toString()}{" "}
                {EstadoEntrevista}s
              </div>
              <div className="flex items-center text-red-600 font-bold ml-2">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  strokeWidth={1.5}
                  stroke="currentColor"
                  className="w-4 h-4 mr-1"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M10.125 2.25h-4.5c-.621 0-1.125.504-1.125 1.125v17.25c0 .621.504 1.125 1.125 1.125h12.75c.621 0 1.125-.504 1.125-1.125v-9M10.125 2.25h.375a9 9 0 0 1 9 9v.375M10.125 2.25A3.375 3.375 0 0 1 13.5 5.625v1.5c0 .621.504 1.125 1.125 1.125h1.5a3.375 3.375 0 0 1 3.375 3.375M9 15l2.25 2.25L15 12"
                  />
                </svg>
                {postulaciones!
                  .filter((x) => x.estado == EstadoDescartado)
                  .length.toString()}{" "}
                {EstadoDescartado}s
              </div>
              <div className="flex items-center text-green-600 font-bold ml-2">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  strokeWidth={1.5}
                  stroke="currentColor"
                  className="w-4 h-4 mr-1"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M10.125 2.25h-4.5c-.621 0-1.125.504-1.125 1.125v17.25c0 .621.504 1.125 1.125 1.125h12.75c.621 0 1.125-.504 1.125-1.125v-9M10.125 2.25h.375a9 9 0 0 1 9 9v.375M10.125 2.25A3.375 3.375 0 0 1 13.5 5.625v1.5c0 .621.504 1.125 1.125 1.125h1.5a3.375 3.375 0 0 1 3.375 3.375M9 15l2.25 2.25L15 12"
                  />
                </svg>
                {postulaciones!
                  .filter((x) => x.estado == EstadoFinalizado)
                  .length.toString()}{" "}
                {EstadoFinalizado}s
              </div>
            </div>
          )}
          <div className="uai-shadow relative overflow-x-auto mt-4">
            <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
              <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                <tr>
                  <th scope="col" className="px-6 py-3">
                    Fecha
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Empresa
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Vacante
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Postulante
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Estado
                  </th>
                </tr>
              </thead>
              <tbody>
                {postulaciones &&
                  postulaciones.map((x, index) => {
                    return (
                      <tr
                        key={index}
                        className="bg-white dark:bg-white dark:border-gray-700"
                      >
                        <td className="px-6 py-4 flex font-bold">
                          <Link
                            href={`/admin/postulaciones/[id]`}
                            as={`/admin/postulaciones/${x.id}`}
                          >
                            {formatearFecha(x.fecha, true)}
                          </Link>
                        </td>
                        <td className="px-6 py-4">{x.empresa}</td>
                        <td className="px-6 py-4">{x.titulo}</td>
                        <td className="px-6 py-4">{x.postulante}</td>
                        <td className="px-6 py-4">{x.estado}</td>
                      </tr>
                    );
                  })}
                {!postulaciones && (
                  <tr className="bg-white border-b dark:bg-white dark:border-gray-700">
                    <td colSpan={6}>
                      <Spinner />
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
        <div className="flex mt-2">
          <button onClick={backButtonClick} type="button" className="boton">
            Volver
          </button>
        </div>
        {mensajeModal && (
          <Modal
            mostrar={!!mensajeModal}
            titulo={tituloModal}
            onCambiarModal={limpiarModal}
          >
            <p>{mensajeModal}</p>
          </Modal>
        )}
      </div>
    </Layout>
  );
}
