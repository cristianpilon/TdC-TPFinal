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
              <h1 className="Titulo1">Gestión de currículums</h1>
              <div>
                {userLayout && (
                  <>
                    <button onClick={miCvClick} className="boton">
                      Mi CV
                    </button>
                    <button
                      onClick={auditoriaPostulacionesClick}
                      className="boton"
                    >
                      Mis Postulaciones
                    </button>
                  </>
                )}
                {!userLayout && rolUsuario === "Administrador" && (
                  <>
                    <button
                      onClick={auditoriasAplicacionClick}
                      className="boton"
                    >
                      Auditorias
                    </button>
                    <button onClick={respaldosClick} className="boton">
                      Respaldos
                    </button>
                    <button className="boton" onClick={permisosClick}>
                      Permisos
                    </button>
                    <button className="boton" onClick={noImplementadoClick}>
                      Usuarios
                    </button>
                  </>
                )}
                <button className="boton" onClick={noImplementadoClick}>
                  Mi cuenta
                </button>
                <button className="boton" onClick={cerrarSesionClick}>
                  Cerrar sesión
                </button>
              </div>
            </nav>
          </header>
          <main className="flex-1">{children}</main>
          <footer>
            <div>
              &copy; Trabajo de Campo/Diploma - Profesor: Pablo Audoglio
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
