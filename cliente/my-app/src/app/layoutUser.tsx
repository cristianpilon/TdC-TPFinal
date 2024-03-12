"use client";
import "./globals.css";
import { useRouter } from "next/navigation";
import Modal from "@/componentes/compartido/modal";
import { useEffect, useState } from "react";
import { obtenerTokenSesion } from "@/componentes/compartido";

export default function Layout({
  children,
  userLayout,
}: {
  children: React.ReactNode;
  userLayout: boolean;
}) {
  const { push } = useRouter();
  const [mostrarModal, setMostrarModal] = useState<boolean>(false);

  useEffect(() => {
    const token = obtenerTokenSesion();
    if (!token) {
      push("/");
    }
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
  const miCuentaClick = () => cambiarModal();
  const cerrarSesionClick = () => push("/");

  return (
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
                <button onClick={auditoriaPostulacionesClick} className="boton">
                  Mis Postulaciones
                </button>
              </>
            )}
            {!userLayout && (
              <button onClick={auditoriasAplicacionClick} className="boton">
                Auditorias
              </button>
            )}
            {!userLayout && (
              <button onClick={respaldosClick} className="boton">
                Respaldos
              </button>
            )}
            <button className="boton" onClick={miCuentaClick}>
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
        <div>&copy; Trabajo de Campo/Diploma - Profesor: Pablo Audoglio</div>
      </footer>
      <Modal
        mostrar={mostrarModal}
        titulo={"En implementación"}
        onCambiarModal={cambiarModal}
      >
        <p>Funcionalidad en desarrollo</p>
      </Modal>
    </>
  );
}
