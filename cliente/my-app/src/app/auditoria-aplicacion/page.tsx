"use client";
import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import Layout from "../layoutUser";
import { fetchPrivado, formatearFecha } from "@/componentes/compartido";
import Modal from "@/componentes/compartido/modal";
import { mensajeErrorGeneral } from "@/constants";
import Spinner from "@/componentes/compartido/spinner";

export default function AuditoriaAplicacion() {
  const { push } = useRouter();
  const [mensajeModal, setMensajeModal] = useState<string>();
  const [tituloModal, setTituloModal] = useState<string>("");
  const [auditorias, setAuditorias] = useState<
    Array<{
      usuario: string;
      ruta: string;
      accion: string;
      fecha: Date;
    }>
  >();

  useEffect(() => {
    obtenerAuditorias();
  }, []);

  const obtenerAuditorias = async () => {
    await fetchPrivado("http://localhost:4000/auditorias/", "GET")
      .then(async (data) => {
        if (data.ok) {
          const respuesta: {
            auditoriasAplicacion: {
              usuario: string;
              ruta: string;
              accion: string;
              fecha: Date;
            }[];
          } = await data.json();

          setAuditorias(respuesta.auditoriasAplicacion);
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

  const backButtonClick = async () => {
    push("/empleos");
  };

  const limpiarModal = () => setMensajeModal(undefined);

  return (
    <Layout>
      <div className="cuerpo">
        <div className="uai-shadow p-2 my-4">
          <h2 className="font-bold mb-2 inline">Auditoria aplicación</h2>
          <div className="uai-shadow relative overflow-x-auto mt-4">
            <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
              <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                <tr>
                  <th scope="col" className="px-6 py-3">
                    Fecha
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Ruta
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Accion
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Usuario
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
                        <td className="px-6 py-4 flex">
                          {formatearFecha(x.fecha, true)}
                        </td>
                        <td className="px-6 py-4">{x.ruta}</td>
                        <td className="px-6 py-4">{x.accion}</td>
                        <td className="px-6 py-4">{x.usuario}</td>
                      </tr>
                    );
                  })}
                {!auditorias && (
                  <tr className="bg-white border-b dark:bg-white dark:border-gray-700">
                    <td colSpan={4}>
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
