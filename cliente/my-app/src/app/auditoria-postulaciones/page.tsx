"use client";
import { useState, useEffect } from "react";
import Layout from "../layoutUser";
import { fetchPrivado, formatearFecha } from "@/componentes/compartido";
import Modal from "@/componentes/compartido/modal";
import { mensajeErrorGeneral } from "@/constants";
import Spinner from "@/componentes/compartido/spinner";

export default function AuditoriaPostulaciones() {
  const [mensajeModal, setMensajeModal] = useState<string>();
  const [tituloModal, setTituloModal] = useState<string>("");
  const [auditorias, setAuditorias] = useState<
    Array<{
      empresa: string;
      titulo: string;
      ubicacion: string;
      fecha: Date;
      estado: string;
    }>
  >();

  useEffect(() => {
    obtenerAuditorias();
  }, []);

  const obtenerAuditorias = async () => {
    await fetchPrivado("http://localhost:4000/postulaciones/", "GET")
      .then(async (data) => {
        if (data.ok) {
          const respuesta: {
            postulaciones: {
              empleo: {
                empresa: string;
                titulo: string;
                ubicacion: string;
              };
              estado: string;
              fecha: Date;
            }[];
          } = await data.json();

          const postulacionesFormateadas = respuesta.postulaciones.map((x) => ({
            empresa: x.empleo.empresa,
            titulo: x.empleo.titulo,
            ubicacion: x.empleo.ubicacion,
            fecha: x.fecha,
            estado: x.estado,
          }));

          setAuditorias(postulacionesFormateadas);
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

  const limpiarModal = () => setMensajeModal(undefined);

  return (
    <Layout userLayout={true}>
      <div className="cuerpo">
        <div className="uai-shadow p-2 my-4">
          <h2 className="font-bold mb-2 inline">Posulaciones</h2>
          <div className="uai-shadow relative overflow-x-auto mt-4">
            <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
              <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                <tr>
                  <th scope="col" className="px-6 py-3">
                    Empresa
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Titulo
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Ubicacion
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Fecha
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Estado
                  </th>
                </tr>
              </thead>
              <tbody>
                {auditorias &&
                  auditorias.map((x, index) => {
                    return (
                      <tr
                        key={index}
                        className="bg-white dark:bg-white dark:border-gray-700"
                      >
                        <td className="px-6 py-4">{x.empresa}</td>
                        <td className="px-6 py-4">{x.titulo}</td>
                        <td className="px-6 py-4">{x.ubicacion}</td>
                        <td className="px-6 py-4 flex">
                          {formatearFecha(x.fecha, true)}
                        </td>
                        <td className="px-6 py-4">{x.estado}</td>
                      </tr>
                    );
                  })}
                {!auditorias && (
                  <tr className="bg-white border-b dark:bg-white dark:border-gray-700">
                    <td colSpan={5}>
                      <Spinner />
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
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
