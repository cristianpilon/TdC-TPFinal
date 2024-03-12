"use client";
import { useState } from "react";
import { useRouter } from "next/navigation";
import Image from "next/image";
import { mensajeErrorGeneral } from "@/constants";
import imgLogin from "../../public/home/logo-uai.svg";
import imgUser from "../../public/home/17004.png";
import imgPassword from "../../public/home/security-system.png";
import {
  guardarTokenSesion,
  guardarRolUsuario,
} from "@/componentes/compartido";

export default function Home() {
  const [usuario, setUsuario] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [mensajeError, setMensajeError] = useState<string>("");
  const [accionesDeshabilitadas, setAccionesDeshabilitadas] =
    useState<boolean>(false);

  const { push } = useRouter();

  const ingresarClick = async () => {
    setAccionesDeshabilitadas(true);
    await fetch("http://localhost:4000/usuarios/validar", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ usuario, password }),
    })
      .then(async (data) => {
        if (data.ok) {
          const respuesta = await data.json();
          guardarTokenSesion(respuesta.token);
          guardarRolUsuario(respuesta.rol);

          if (
            respuesta.rol === "Administrador" ||
            respuesta.rol === "Reclutador"
          ) {
            push("/postulaciones");
          } else {
            push("/empleos");
          }
          return;
        } else if (data.status === 400) {
          setMensajeError("El usuario o la contraseña son incorrectos");
          setAccionesDeshabilitadas(false);
          return;
        }

        setMensajeError(mensajeErrorGeneral);
      })
      .catch(() => {
        setMensajeError(mensajeErrorGeneral);
        setAccionesDeshabilitadas(false);
      });
  };

  return (
    <>
      <div id="login-back">
        <header className="bg-primary navbar navbar-dark p-4 uai-bg">
          <Image
            priority={true}
            src={imgLogin}
            className="navbar-brand miuai-logo"
            fill={false}
            alt=""
          />
          <div className="logo-miuai d-none d-sm-block">
            mi<b>UAI</b>
          </div>
        </header>
        <main>
          <div className="imgDiv">
            <label>Completá tus datos para iniciar sesión</label>
            {mensajeError && (
              <div
                className="bg-red-200 border border-red-400 text-red-800 px-4 py-3 relative mt-3"
                role="alert"
              >
                <span className="block text-red-700 sm:inline mr-8 mt-10">
                  {mensajeError}
                </span>
                <span
                  className="absolute top-0 bottom-0 right-0 px-2 py-3"
                  onClick={() => {
                    setMensajeError("");
                  }}
                >
                  <svg
                    className="fill-current h-6 w-6 text-red-700"
                    role="button"
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 20 20"
                  >
                    <title>Close</title>
                    <path d="M14.348 14.849a1.2 1.2 0 0 1-1.697 0L10 11.819l-2.651 3.029a1.2 1.2 0 1 1-1.697-1.697l2.758-3.15-2.759-3.152a1.2 1.2 0 1 1 1.697-1.697L10 8.183l2.651-3.031a1.2 1.2 0 1 1 1.697 1.697l-2.758 3.152 2.758 3.15a1.2 1.2 0 0 1 0 1.698z" />
                  </svg>
                </span>
              </div>
            )}

            <form action="" method="post">
              <div className="input1">
                <div className="Username">
                  <Image
                    className="imgUser"
                    src={imgUser}
                    fill={false}
                    alt=""
                  />
                  <input
                    type="text"
                    id="user"
                    name="user"
                    value={usuario}
                    onChange={(e) => setUsuario(e.target.value)}
                    placeholder="Username"
                    required
                  />
                </div>
              </div>

              <div className="input2">
                <div className="Username">
                  <Image
                    className="imgUser"
                    src={imgPassword}
                    fill={false}
                    alt=""
                  />
                  <input
                    type="password"
                    id="psw"
                    name="psw"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    placeholder="Introduza la contraseña"
                    required
                  />
                </div>
              </div>
              <div className="remember-box flex">
                <div>
                  <label className="p1">
                    <input type="checkbox" value="" />
                    Mantener mi sesión iniciada
                  </label>
                </div>
                <div>
                  <a className="p2" href="#">
                    ¿Olvidó contraseña?
                  </a>
                </div>
              </div>

              <div className="botonera">
                <input
                  disabled={
                    accionesDeshabilitadas || usuario === "" || password === ""
                  }
                  type="submit"
                  className="boton btn-primary"
                  value="Ingresar"
                  onClick={ingresarClick}
                />
                <input
                  type="submit"
                  className="boton btn-secondary"
                  value="Registrar"
                />
              </div>
            </form>
          </div>
        </main>
      </div>
    </>
  );
}
