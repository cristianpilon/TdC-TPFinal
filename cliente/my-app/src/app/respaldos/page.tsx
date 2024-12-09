"use client";
import { useState, useEffect } from "react";
import Layout from "../layoutUser";
import { useRouter } from "next/navigation";
import { fetchPrivado, formatearFecha } from "@/componentes/compartido";
import Modal from "@/componentes/compartido/modal";
import { mensajeErrorGeneral } from "@/constants";
import Spinner from "@/componentes/compartido/spinner";

export default function Respaldos() {
  const { push } = useRouter();
  const [mensajeModal, setMensajeModal] = useState<string>();
  const [tituloModal, setTituloModal] = useState<string>("");
  const [tipoRespaldo, setTipoRespaldo] = useState<string>("Completo");
  const [respaldos, setRespaldos] =
    useState<Array<{ fecha: Date; tipo: string }>>();

  useEffect(() => {
    obtenerRespaldos();
  }, []);

  const obtenerRespaldos = async () => {
    await fetchPrivado("http://localhost:4000/respaldos/", "GET")
      .then(async (data) => {
        if (data.ok) {
          const respuesta: { respaldos: { fecha: Date; tipo: string }[] } =
            await data.json();

          setRespaldos(respuesta.respaldos);
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

  const clickCrearRespaldo = async () => {
    await fetchPrivado(
      "http://localhost:4000/respaldos/",
      "POST",
      JSON.stringify({
        tipoRespaldo,
      })
    )
      .then(async (data) => {
        if (data.ok) {
          setTituloModal("Respaldo");
          setMensajeModal("Se ha creado correctamente el respaldo");
          obtenerRespaldos();
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

  const clickEliminarRespaldo = async (index: number) => {
    if (!respaldos) {
      return;
    }

    await fetchPrivado(
      "http://localhost:4000/respaldos/",
      "DELETE",
      JSON.stringify({
        tipo: respaldos[index].tipo,
        fecha: new Date(respaldos[index].fecha),
      })
    )
      .then(async (data) => {
        if (data.ok) {
          setTituloModal("Respaldo");
          setMensajeModal("Se ha eliminado correctamente el respaldo");
          obtenerRespaldos();
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

  const clickRestaurarRespaldo = async (index: number) => {
    if (!respaldos) {
      return;
    }

    await fetchPrivado(
      "http://localhost:4000/respaldos/",
      "PUT",
      JSON.stringify({
        tipo: respaldos[index].tipo,
        fecha: new Date(respaldos[index].fecha),
      })
    )
      .then(async (data) => {
        if (data.ok) {
          // Redirecciono al login
          push("/");
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

  const onTipoRespaldoChange = (e: React.FormEvent<HTMLInputElement>) => {
    setTipoRespaldo(e.currentTarget.value);
  };

  const backButtonClick = async () => {
    push("/admin");
  };

  const limpiarModal = () => setMensajeModal(undefined);

  return (
    <Layout>
      <div className="cuerpo">
        <div className="uai-shadow p-2 my-4">
          <h2 className="font-bold mb-2 inline">Respaldos</h2>
          <div className="flex mt-2">
            <button
              className="boton flex"
              style={{
                minWidth: "auto",
                minHeight: "auto",
                padding: "5px",
                display: "flex",
              }}
              onClick={clickCrearRespaldo}
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
            <div className="flex ml-4">
              <div>
                <label className="flex">
                  <input
                    checked={tipoRespaldo === "Completo"}
                    className="mr-1"
                    name="tipoRespaldo"
                    type="radio"
                    value="Completo"
                    onChange={onTipoRespaldoChange}
                  />
                  Completo
                </label>
              </div>
              <div className="ml-4">
                <label className="flex">
                  <input
                    checked={tipoRespaldo === "Diferencial"}
                    className="mr-1"
                    name="tipoRespaldo"
                    type="radio"
                    value="Diferencial"
                    onChange={onTipoRespaldoChange}
                  />
                  Diferencial
                </label>
              </div>
            </div>
          </div>
          <div className="uai-shadow relative overflow-x-auto mt-4">
            <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
              <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                <tr>
                  <th scope="col" className="px-6 py-3">
                    Fecha
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Tipo
                  </th>
                  <th scope="col" className="px-6 py-3">
                    Acciones
                  </th>
                </tr>
              </thead>
              <tbody>
                {respaldos &&
                  respaldos.map((x, index) => {
                    return (
                      <tr
                        key={index}
                        className="bg-white dark:bg-white dark:border-gray-700"
                      >
                        <td className="px-6 py-4 flex">
                          {formatearFecha(x.fecha, true)}
                        </td>
                        <td className="px-6 py-4">{x.tipo}</td>

                        <td className="px-6 py-4">
                          <button
                            className="boton mr-2"
                            title="Restaurar"
                            style={{
                              minWidth: "auto",
                              minHeight: "auto",
                              padding: "5px",
                            }}
                            onClick={() => {
                              clickRestaurarRespaldo(index);
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
                                d="M20.25 6.375c0 2.278-3.694 4.125-8.25 4.125S3.75 8.653 3.75 6.375m16.5 0c0-2.278-3.694-4.125-8.25-4.125S3.75 4.097 3.75 6.375m16.5 0v11.25c0 2.278-3.694 4.125-8.25 4.125s-8.25-1.847-8.25-4.125V6.375m16.5 0v3.75m-16.5-3.75v3.75m16.5 0v3.75C20.25 16.153 16.556 18 12 18s-8.25-1.847-8.25-4.125v-3.75m16.5 0c0 2.278-3.694 4.125-8.25 4.125s-8.25-1.847-8.25-4.125"
                              />
                            </svg>
                          </button>
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
                              clickEliminarRespaldo(index);
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
                {!respaldos && (
                  <tr className="bg-white border-b dark:bg-white dark:border-gray-700">
                    <td colSpan={3}>
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
