"use client";
import "./globals.css";
import { useRouter } from "next/navigation";
import Modal from "@/componentes/compartido/modal";
import { useEffect, useRef, useState } from "react";
import {
  eliminarSesionStorage,
  fetchPrivado,
  obtenerRolUsuario,
  obtenerTokenSesion,
} from "@/componentes/compartido";
import Spinner from "@/componentes/compartido/spinner";

export default function Layout({ children }: { children: React.ReactNode }) {
  const { push } = useRouter();
  const [mostrarModal, setMostrarModal] = useState<boolean>(false);
  const [rolUsuario, setRolUsuario] = useState<string>();
  const [notificaciones, setNotificaciones] = useState<
    {
      id: number;
      mensaje: string;
      fechaCreacion: Date;
      fechaLectura?: Date;
    }[]
  >();
  const [mostrarNotificationes, setMostrarNotificationes] =
    useState<boolean>(false);

  const timerTimeout = useRef<NodeJS.Timeout[]>([]);

  useEffect(() => {
    const token = obtenerTokenSesion();
    const rol = obtenerRolUsuario();
    if (!token || !rol) {
      push("/");
    }

    if (rol === null) {
      return;
    }

    setRolUsuario(rol);

    suscribirNotificaciones();

    return () => {
      timerTimeout.current.forEach((timeout) => clearTimeout(timeout));
      timerTimeout.current = [];
    };

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const suscribirNotificaciones = (fechaLimite?: Date) => {
    timerTimeout.current.forEach((timeout) => clearTimeout(timeout));
    timerTimeout.current = [];

    const newTimeout = setTimeout(async () => {
      let fechaUltimaNotificacion: string = "";
      if (fechaLimite) {
        fechaUltimaNotificacion = `limite=${fechaLimite.toISOString()}`;
      }

      await fetchPrivado(
        `http://localhost:4000/notificaciones${fechaUltimaNotificacion}`,
        "GET"
      )
        .then(async (data) => {
          if (data.ok) {
            const respuesta = await data.json();
            cargarNotificaciones(respuesta);

            return;
          }
        })
        .finally(() => {
          suscribirNotificaciones();
        });
    }, 3000);

    timerTimeout.current.push(newTimeout);
  };

  const cargarNotificaciones = (datos: any) => {
    const nuevasNotificaciones = datos.notificaciones.map(
      (x: {
        id: number;
        mensaje: string;
        fechaCreacion: Date;
        fechaLectura?: Date;
      }) => ({
        id: x.id,
        mensaje: x.mensaje,
        fechaCreacion: new Date(x.fechaCreacion),
        fechaLectura: x.fechaLectura ? new Date(x.fechaLectura) : undefined,
      })
    );

    setNotificaciones((notificacionesExistentes) => {
      const actualizadas = [...(notificacionesExistentes || [])];

      nuevasNotificaciones.forEach(
        (nueva: {
          id: number;
          mensaje: string;
          fechaCreacion: Date;
          fechaLectura?: Date;
        }) => {
          const index = actualizadas.findIndex(
            (existente) => existente.id === nueva.id
          );

          if (index !== -1) {
            // Si existe, actualizar el registro
            actualizadas[index] = nueva;
          } else {
            // Si no existe, agregarlo
            actualizadas.push(nueva);
          }
        }
      );

      return actualizadas;
    });
  };

  const cambiarModal = () => {
    const mostrar = !mostrarModal;
    setMostrarModal(mostrar);
  };

  const auditoriasAplicacionClick = () => push("/auditoria-aplicacion");
  const respaldosClick = () => push("/respaldos");
  const miCvClick = () => push("/cv");
  const auditoriaPostulacionesClick = () => push("/auditoria-postulaciones");
  const permisosClick = () => push("/admin/permisos");
  const noImplementadoClick = () => cambiarModal();

  const notificacionesClick = async () => {
    const nuevoValor = !mostrarNotificationes;
    setMostrarNotificationes(nuevoValor);

    timerTimeout.current.forEach((timeout) => clearTimeout(timeout));
    timerTimeout.current = [];

    if (nuevoValor) {
      await fetchPrivado(`http://localhost:4000/notificaciones`, "PUT").finally(
        async () => {
          suscribirNotificaciones();
        }
      );
      return;
    }

    suscribirNotificaciones();
  };

  const inicioClick = () => {
    if (rolUsuario === "Administrador") {
      push("/admin");
    } else {
      push("/empleos");
    }
  };

  const cerrarSesionClick = () => {
    eliminarSesionStorage();
    push("/");
  };

  return (
    <>
      {!rolUsuario && <Spinner />}
      {rolUsuario && (
        <>
          <header className="uai-bg">
            <nav>
              <h1 className="Titulo1">UAI Talent Hub</h1>
              <div>
                <ul className="flex">
                  <li>
                    <button
                      className="boton text-black"
                      onClick={inicioClick}
                      title="Inicio"
                      style={{ minWidth: "auto", padding: "6px" }}
                      data-dropdown-toggle="dropdownNavbar"
                    >
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        strokeWidth={1.5}
                        stroke="currentColor"
                        className="size-3 h-6 w-6 p-0"
                      >
                        <path
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          d="m2.25 12 8.954-8.955c.44-.439 1.152-.439 1.591 0L21.75 12M4.5 9.75v10.125c0 .621.504 1.125 1.125 1.125H9.75v-4.875c0-.621.504-1.125 1.125-1.125h2.25c.621 0 1.125.504 1.125 1.125V21h4.125c.621 0 1.125-.504 1.125-1.125V9.75M8.25 21h8.25"
                        />
                      </svg>
                    </button>
                  </li>
                  {rolUsuario === "Usuario" && (
                    <>
                      <li>
                        <button onClick={miCvClick} className="boton">
                          Mi CV
                        </button>
                      </li>
                      <li>
                        <button
                          onClick={auditoriaPostulacionesClick}
                          className="boton text-black"
                        >
                          Mis Postulaciones
                        </button>
                      </li>
                    </>
                  )}
                  {rolUsuario !== "Usuario" &&
                    rolUsuario === "Administrador" && (
                      <>
                        <li>
                          <button
                            onClick={auditoriasAplicacionClick}
                            className="boton text-black"
                          >
                            Auditorias
                          </button>
                        </li>
                        <li>
                          <button
                            onClick={respaldosClick}
                            className="boton text-black"
                          >
                            Respaldos
                          </button>
                        </li>
                        <li>
                          <button
                            className="boton text-black"
                            onClick={permisosClick}
                          >
                            Permisos
                          </button>
                        </li>
                        <li>
                          <button
                            className="boton text-black"
                            onClick={noImplementadoClick}
                          >
                            Usuarios
                          </button>
                        </li>
                      </>
                    )}
                  <li>
                    <button
                      className="boton text-black"
                      onClick={noImplementadoClick}
                    >
                      Mi cuenta
                    </button>
                  </li>
                  <li className="relative">
                    <button
                      className="boton text-black"
                      onClick={notificacionesClick}
                      title="Notificaciones"
                      style={{ minWidth: "auto", padding: "6px" }}
                      data-dropdown-toggle="dropdownNavbar"
                    >
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        strokeWidth={1.5}
                        stroke="currentColor"
                        className="size-3 h-6 w-6 p-0"
                      >
                        <path
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          d="M14.857 17.082a23.848 23.848 0 0 0 5.454-1.31A8.967 8.967 0 0 1 18 9.75V9A6 6 0 0 0 6 9v.75a8.967 8.967 0 0 1-2.312 6.022c1.733.64 3.56 1.085 5.455 1.31m5.714 0a24.255 24.255 0 0 1-5.714 0m5.714 0a3 3 0 1 1-5.714 0"
                        />
                      </svg>
                    </button>
                    {notificaciones &&
                      !mostrarNotificationes &&
                      notificaciones.filter((x) => !x.fechaLectura).length >
                        0 && (
                        <div
                          onClick={notificacionesClick}
                          className="rounded-full bg-red-600 text-white burbuja-notificacion"
                        >
                          {notificaciones.filter((x) => !x.fechaLectura).length}
                        </div>
                      )}
                    {mostrarNotificationes && (
                      <div
                        className="listado-notificaciones absolute right-0 z-10 mt-2 origin-top-right rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none w-[300px] max-h-[450px] overflow-y-auto"
                        role="menu"
                        aria-orientation="vertical"
                        aria-labelledby="menu-button"
                        tabIndex={-1}
                      >
                        <div className="py-1" role="none">
                          <span
                            className="text-lg border-b-2 font-bold block px-4 py-2 text-gray-700"
                            role="menuitem"
                            tabIndex={-1}
                            id="menu-item-0"
                          >
                            Notificaciones
                          </span>
                          {!notificaciones && <Spinner />}
                          {notificaciones && notificaciones.length === 0 && (
                            <span
                              className="block px-4 py-2 text-sm text-gray-700"
                              role="menuitem"
                              tabIndex={-1}
                              id="menu-item-1"
                            >
                              No hay notificaciones
                            </span>
                          )}
                          {notificaciones && notificaciones.length > 0 && (
                            <>
                              {notificaciones
                                .sort(
                                  (a, b) =>
                                    b.fechaCreacion.getTime() -
                                    a.fechaCreacion.getTime()
                                )
                                .map((x, index) => (
                                  <span
                                    className={`item-notificacion block px-4 py-2 text-sm ${
                                      index !== 0 ? "border-t-2" : ""
                                    } text-gray-700`}
                                    role="menuitem"
                                    tabIndex={-1}
                                    dangerouslySetInnerHTML={{
                                      __html: x.mensaje,
                                    }}
                                  ></span>
                                ))}
                              {notificaciones.length > 7 && (
                                <span
                                  className="block px-4 py-2 text-sm border-t-2 text-gray-700 text-center"
                                  role="menuitem"
                                  tabIndex={-1}
                                  id="menu-item-1"
                                >
                                  <strong
                                    onClick={() => {
                                      suscribirNotificaciones(
                                        notificaciones[
                                          notificaciones.length - 1
                                        ].fechaCreacion
                                      );
                                    }}
                                    className="cursor-pointer"
                                  >
                                    Mostrar más
                                  </strong>
                                </span>
                              )}
                            </>
                          )}
                        </div>
                      </div>
                    )}
                  </li>
                  <li>
                    <button
                      className="boton text-black"
                      onClick={cerrarSesionClick}
                    >
                      Cerrar sesión
                    </button>
                  </li>
                </ul>
              </div>
            </nav>
          </header>
          <main className="flex-1">{children}</main>
          <footer>
            <div>
              &copy; Trabajo Final de Ingeniería - Profesores: Silvia Poncio /
              Pablo Audoglio
            </div>
          </footer>
          <Modal
            mostrar={mostrarModal}
            titulo={"En implementación"}
            onCambiarModal={cambiarModal}
          >
            <p>Funcionalidad en desarrollo</p>
          </Modal>
        </>
      )}
    </>
  );
}
