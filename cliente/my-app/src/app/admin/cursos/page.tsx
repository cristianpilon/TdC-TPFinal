"use client";
import Link from "next/link";
import { useRouter } from "next/navigation";
import Layout from "../../layoutUser";
import Modal from "@/componentes/compartido/modal";
import { useEffect, useState } from "react";
import { fetchPrivado, formatearFecha } from "@/componentes/compartido";
import Spinner from "@/componentes/compartido/spinner";
import { mensajeErrorGeneral } from "@/constants";

export default function Cursos() {
  const { push } = useRouter();
  const [mensajeModal, setMensajeModal] = useState<string>();
  const [tituloModal, setTituloModal] = useState<string>("");
  const [cursos, setCursos] = useState<
    Array<{
      id: number;
      fecha: Date;
      empresa: string;
      titulo: string;
    }>
  >();

  useEffect(() => {
    const obtenerCursos = async () => {
      await fetchPrivado("http://localhost:4000/cursos/", "GET")
        .then(async (data) => {
          if (data.ok) {
            const respuesta: {
              cursos: {
                id: number;
                fecha: Date;
                empresa: string;
                titulo: string;
              }[];
            } = await data.json();

            const cursosFormateados = respuesta.cursos.map((x) => ({
              id: x.id,
              empresa: x.empresa,
              titulo: x.titulo,
              fecha: x.fecha,
            }));

            setCursos(cursosFormateados);
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

    obtenerCursos();
  }, []);

  const crearCursoClick = async () => {
    push("/admin/cursos/nuevo");
  };

  const backButtonClick = async () => {
    push("/admin");
  };

  const limpiarModal = () => setMensajeModal(undefined);
  const clickEliminarCurso = (index: number) => setMensajeModal(undefined);

  return (
    <Layout>
      <div className="cuerpo">
        <div className="uai-shadow p-2 my-4">
          <div className="flex mt-2">
            <h2 className="font-bold inline">Cursos</h2>
            <button
              className="boton flex ml-3"
              style={{
                minWidth: "auto",
                minHeight: "auto",
                padding: "5px",
                display: "flex",
              }}
              onClick={crearCursoClick}
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth="1.5"
                stroke="currentColor"
                className="w-4 h-4 mr-1"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M12 4.5v15m7.5-7.5h-15"
                />
              </svg>
              Crear
            </button>
          </div>
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
                    Título
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Acciones
                  </th>
                </tr>
              </thead>
              <tbody>
                {cursos &&
                  cursos.map((x, index) => {
                    return (
                      <tr
                        key={index}
                        className="bg-white dark:bg-white dark:border-gray-700"
                      >
                        <td className="px-6 py-4 flex font-bold">
                          <Link
                            href={`/admin/cursos/[id]`}
                            as={`/admin/cursos/${x.id}`}
                          >
                            {formatearFecha(x.fecha, true)}
                          </Link>
                        </td>
                        <td className="px-6 py-4">{x.empresa}</td>
                        <td className="px-6 py-4">{x.titulo}</td>

                        <td className="px-6 py-4">
                          <button
                            className="boton"
                            title="Eliminar"
                            style={{
                              minWidth: "auto",
                              minHeight: "auto",
                              color: "red",
                              padding: "5px",
                            }}
                            onClick={() => {
                              clickEliminarCurso(x.id);
                            }}
                          >
                            <svg
                              xmlns="http://www.w3.org/2000/svg"
                              fill="none"
                              viewBox="0 0 24 24"
                              strokeWidth="1.5"
                              stroke="currentColor"
                              className="w-3 h-3"
                            >
                              <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                d="M6 18 18 6M6 6l12 12"
                              />
                            </svg>
                          </button>
                        </td>
                      </tr>
                    );
                  })}
                {!cursos && (
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
