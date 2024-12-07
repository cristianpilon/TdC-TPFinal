export const obtenerTokenSesion = () => {
  return sessionStorage.getItem("token");
};

export const guardarTokenSesion = (token: any) => {
  const tokenString = JSON.stringify(token);
  return sessionStorage.setItem("token", tokenString);
};

export const obtenerRolUsuario = () => {
  return sessionStorage.getItem("rolUsuario");
};

export const guardarRolUsuario = (rol: string) => {
  return sessionStorage.setItem("rolUsuario", rol);
};

export const eliminarSesionStorage = () => {
  sessionStorage.removeItem("rolUsuario");
  sessionStorage.removeItem("token");
};

export const fetchPrivado = (
  url: string,
  metodo: string,
  cuerpo: string = "",
  encabezados: any = {}
) => {
  const jwt = obtenerTokenSesion();

  return fetch(url, {
    method: metodo,
    headers: {
      ...encabezados,
      Authorization: `bearer ${jwt}`,
      "Content-Type": "application/json",
    },
    body: cuerpo ? cuerpo : undefined,
  });
};

export const formatearFecha = (fecha: Date, incluirHora: boolean = false) => {
  fecha = new Date(fecha);
  const yyyy = fecha.getFullYear();
  let mm: number | string = fecha.getMonth() + 1;
  let dd: number | string = fecha.getDate();

  if (dd < 10) dd = "0" + dd;
  if (mm < 10) mm = "0" + mm;

  let hora: string = "";

  if (incluirHora) {
    hora += " " + fecha.toTimeString().split(" ")[0];
  }

  return dd + "/" + mm + "/" + yyyy + hora;
};
