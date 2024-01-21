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
