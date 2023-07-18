"use client";
import './globals.css'
import { useRouter } from 'next/navigation';

export default function Layout({ children, userLayout }: {
  children: React.ReactNode,
  userLayout: boolean,
}) {
  const { push } = useRouter();

  const notImplementedAlert = () => alert('Funcionalidad en desarrollo')
  const cerrarSesionClick = () => push('/') 

  return (
    <>
      <header>
        <nav>
          <h1 className="Titulo1">Gestión de currículums</h1>
          <div>
              
              {userLayout && (<button onClick={notImplementedAlert} className="boton">Mi CV</button>)} 
              <button className="boton" onClick={notImplementedAlert}>Mi cuenta</button>
              <button className="boton" onClick={cerrarSesionClick}>Cerrar sesión</button>
          </div>
        </nav>
      </header>
      <main>{children}</main>
      <footer>&copy; Cristian Pilon - Paula Fernandez. TP Trabajo de Campo - Pablo Audoglio</footer>
    </>
  );
}