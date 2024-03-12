"use client";
import { useState, useEffect } from "react";
import Layout from "../layoutUser";
import { useRouter } from "next/navigation";
import { fetchPrivado } from "@/componentes/compartido";
import Modal from "@/componentes/compartido/modal";
import { mensajeErrorGeneral } from "@/constants";
import Spinner from "@/componentes/compartido/spinner";

export default function Admin() {
  const [titulo, setTitulo] = useState<string>("");
  const [empleos, setEmpleos] = useState<
    Array<{
      id: string;
      titulo: string;
      fechaPublicacion: string;
      ubicacion: string;
      empresa: string;
    }>
  >();

  const { push } = useRouter();
  const [mensajeModal, setMensajeModal] = useState<string>();
  const [tituloModal, setTituloModal] = useState<string>("");

  useEffect(() => {
    obtenerEmpleos();

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const obtenerEmpleos = async () => {
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
          }>;
          const empleosMap = empleos.map<{
            id: string;
            titulo: string;
            fechaPublicacion: string;
            ubicacion: string;
            empresa: string;
          }>(({ id, titulo, fechaPublicacion, ubicacion, empresa }) => ({
            id,
            titulo,
            fechaPublicacion,
            ubicacion,
            empresa,
          }));
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

  // TODO: crear
  const clickCrearEmpleo = async () => {
    await fetchPrivado("http://localhost:4000/empleos/", "POST")
      .then(async (data) => {
        if (data.ok) {
          setTituloModal("Respaldo");
          setMensajeModal("Se ha creado correctamente el respaldo");
          obtenerEmpleos();
          return;
        }

        setTituloModal("Error");
        setMensajeModal(mensajeErrorGeneral);
      })
      .catch(() => {
        setTituloModal("Error");
        setMensajeModal(mensajeErrorGeneral);
      });
  };

  const clickEliminarEmpleo = async (index: number) => {
    if (!empleos) {
      return;
    }

    await fetchPrivado(
      `http://localhost:4000/empleos/${empleos[index].id}`,
      "DELETE"
    )
      .then(async (data) => {
        if (data.ok) {
          setTituloModal("Respaldo");
          setMensajeModal("Se ha eliminado correctamente el empleo");
          obtenerEmpleos();
          return;
        }

        setTituloModal("Error");
        setMensajeModal(mensajeErrorGeneral);
      })
      .catch(() => {
        setTituloModal("Error");
        setMensajeModal(mensajeErrorGeneral);
      });
  };

  const clickBuscar = () => {
    setEmpleos(undefined);
    obtenerEmpleos();
  };
  const limpiarModal = () => setMensajeModal(undefined);

  return (
    <Layout userLayout={false}>
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
              className="ml-0 boton text-white font-bold py-2 px-4"
              onClick={clickBuscar}
            >
              Buscar
            </button>
          </div>
          <div className="relative h-9 basis-1/2 flex justify-end">
            <button
              className="ml-0 boton text-white font-bold py-2 px-4"
              onClick={clickBuscar}
            >
              Postulaciones
            </button>
          </div>
        </div>
        <div className="uai-shadow p-2 my-4">
          <h2 className="font-bold mb-2 inline">Empleos</h2>
          <div className="flex mt-2">
            <button
              className="boton flex"
              style={{
                minWidth: "auto",
                minHeight: "auto",
                padding: "5px",
                display: "flex",
              }}
              onClick={clickCrearEmpleo}
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
                    Empresa
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Título
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Fecha
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Ubicación
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Acciones
                  </th>
                </tr>
              </thead>
              <tbody>
                {empleos &&
                  empleos.map((x, index) => {
                    return (
                      <tr
                        key={index}
                        className="bg-white dark:bg-white dark:border-gray-700"
                      >
                        <td className="px-6 py-4">{x.empresa}</td>
                        <td className="px-6 py-4">{x.titulo}</td>
                        <td className="px-6 py-4">{x.fechaPublicacion}</td>
                        <td className="px-6 py-4">{x.ubicacion}</td>

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
                              clickEliminarEmpleo(index);
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
                {!empleos && (
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
