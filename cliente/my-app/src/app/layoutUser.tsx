"use client";
import "./globals.css";
import { useRouter } from "next/navigation";
import Modal from "@/componentes/compartido/modal";
import { useEffect, useState } from "react";
import {
  eliminarSesionStorage,
  obtenerRolUsuario,
  obtenerTokenSesion,
} from "@/componentes/compartido";
import Spinner from "@/componentes/compartido/spinner";

export default function Layout({
  children,
  userLayout,
}: {
  children: React.ReactNode;
  userLayout: boolean;
}) {
  const { push } = useRouter();
  const [mostrarModal, setMostrarModal] = useState<boolean>(false);
  const [rolUsuario, setRolUsuario] = useState<string>();
  const [mostrarNotificationes, setMostrarNotificationes] =
    useState<boolean>(false);

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
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

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
  const notificacionesClick = () => {
    const nuevoValor = !mostrarNotificationes;
    setMostrarNotificationes(nuevoValor);
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
                  {userLayout && (
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
                  {!userLayout && rolUsuario === "Administrador" && (
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
                    {mostrarNotificationes && (
                      <div
                        className="absolute right-0 z-10 mt-2 w-56 origin-top-right rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none"
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
                          <span
                            className="block px-4 py-2 text-sm text-gray-700"
                            role="menuitem"
                            tabIndex={-1}
                            id="menu-item-1"
                          >
                            No hay notificaciones
                          </span>
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
