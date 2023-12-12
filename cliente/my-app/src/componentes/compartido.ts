export const obtenerTokenSesion = () => {
    return sessionStorage.getItem("token");
}

export const guardarTokenSesion = (token: any) => {
    console.log("guarda token", token);


    const tokenString = JSON.stringify(token);
    console.log("guarda token string", tokenString);
    return sessionStorage.setItem("token", tokenString);
}

export const obtenerRolUsuario = () => {
    return sessionStorage.getItem("rolUsuario");
}

export const guardarRolUsuario = (rol: string) => {
    console.log("guarda rol", rol);

    return sessionStorage.setItem("rolUsuario", rol);
}

export const fetchPrivado = (url: string, metodo: string, cuerpo: string = "", encabezados: any = {}) => {
    const jwt = obtenerTokenSesion();

    return fetch(url, { 
      method: metodo,
      headers: { 
        ...encabezados, 
        Authorization: `bearer ${jwt}` ,
        "Content-Type": "application/json",
    },
      body: cuerpo ? cuerpo : undefined,
    });
}