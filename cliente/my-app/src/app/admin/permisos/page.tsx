"use client";
import { useState, useEffect } from "react";
import { Tab, TabList, TabPanel, Tabs } from "react-tabs";
import Select, { SingleValue } from "react-select";
import Layout from "../../layoutUser";
import { useRouter } from "next/navigation";
import { fetchPrivado } from "@/componentes/compartido";
import Modal from "@/componentes/compartido/modal";
import { mensajeErrorGeneral } from "@/constants";
import Spinner from "@/componentes/compartido/spinner";
import { SelectOpcion } from "@/componentes/compartido/select/selectOpcion";

export default function Admin() {
  const [titulo, setTitulo] = useState<string>("");
  const [usuarios, setUsuarios] = useState<
    Array<{
      id: string;
      nombre: string;
      rol: string;
      grupos: {
        id: number;
        nombre: string;
      };
      permisos: {
        id: number;
        nombre: string;
      };
    }>
  >();
  const [tabSeleccionado, setTabSeleccionado] = useState<number>(0);
  const [selectUsuario, setSelectUsuario] = useState<string>();
  const [selectUsuarios, setSelectUsuarios] = useState<SelectOpcion[]>([
    { label: "Cristian Pilon (cristian.pilon@uai.edu.ar)", value: "4" },
    { label: "Nacho Riveira (ignacio.riveira@uai.edu.ar)", value: "5" },
    { label: "Paula Fernandez (cristian.pilon@uai.edu.ar)", value: "6" },
  ]);
  const [selectGrupo, setSelectGrupo] = useState<string>();
  const [selectGruposUsuario, setSelectGruposUsuario] = useState<
    readonly SelectOpcion[]
  >([]);
  const [selectGruposUsuarioAnterior, setSelectGruposUsuarioAnterior] =
    useState<readonly SelectOpcion[]>([]);
  const [selectGrupos, setSelectGrupos] = useState<SelectOpcion[]>([
    { label: "Administradores", value: "1" },
    { label: "Reclutador", value: "2" },
    { label: "Postulantes", value: "3" },
    { label: "DB Admin", value: "4" },
    { label: "Gestión postulaciones", value: "5" },
    { label: "Gestión empleos", value: "6" },
    { label: "Auditor", value: "7" },
    { label: "Postulaciones personales", value: "8" },
  ]);

  useEffect(() => {
    if (!selectUsuario) {
      return;
    }
    if (selectUsuario === selectUsuarios[0].value) {
      const grupoAdmin = selectGrupos.find((x) => x.value === "1");
      if (!!grupoAdmin) {
        setSelectGruposUsuario([grupoAdmin]);
      }
      const nuevosPermisos = { ...permisos };
      nuevosPermisos[0].escritura = false;
      nuevosPermisos[0].lectura = false;
      nuevosPermisos[1].escritura = false;
      nuevosPermisos[1].lectura = false;
      nuevosPermisos[2].escritura = true;
      nuevosPermisos[2].lectura = true;
      nuevosPermisos[3].escritura = false;
      nuevosPermisos[3].lectura = false;
      nuevosPermisos[4].escritura = false;
      nuevosPermisos[4].lectura = false;
      nuevosPermisos[5].escritura = true;
      nuevosPermisos[5].lectura = true;
      nuevosPermisos[6].escritura = true;
      nuevosPermisos[6].lectura = true;
      nuevosPermisos[7].escritura = true;
      nuevosPermisos[7].lectura = true;
      nuevosPermisos[8].escritura = true;
      nuevosPermisos[8].lectura = true;
      setPermisos(nuevosPermisos);
    } else if (selectUsuario === selectUsuarios[1].value) {
      const grupoReclutador = selectGrupos.find((x) => x.value === "2");
      if (!!grupoReclutador) {
        setSelectGruposUsuario([grupoReclutador]);
      }
      const nuevosPermisos = { ...permisos };
      nuevosPermisos[0].escritura = false;
      nuevosPermisos[0].lectura = false;
      nuevosPermisos[1].escritura = false;
      nuevosPermisos[1].lectura = false;
      nuevosPermisos[2].escritura = true;
      nuevosPermisos[2].lectura = true;
      nuevosPermisos[3].escritura = false;
      nuevosPermisos[3].lectura = false;
      nuevosPermisos[4].escritura = false;
      nuevosPermisos[4].lectura = false;
      nuevosPermisos[5].escritura = false;
      nuevosPermisos[5].lectura = false;
      nuevosPermisos[6].escritura = false;
      nuevosPermisos[6].lectura = false;
      nuevosPermisos[7].escritura = true;
      nuevosPermisos[7].lectura = true;
      nuevosPermisos[8].escritura = true;
      nuevosPermisos[8].lectura = true;
      setPermisos(nuevosPermisos);
    } else if (selectUsuario === selectUsuarios[2].value) {
      const grupoUsuario = selectGrupos.find((x) => x.value === "3");
      if (!!grupoUsuario) {
        setSelectGruposUsuario([grupoUsuario]);
      }
      const nuevosPermisos = { ...permisos };
      nuevosPermisos[0].escritura = true;
      nuevosPermisos[0].lectura = true;
      nuevosPermisos[1].escritura = true;
      nuevosPermisos[1].lectura = true;
      nuevosPermisos[2].escritura = true;
      nuevosPermisos[2].lectura = true;
      nuevosPermisos[3].escritura = true;
      nuevosPermisos[3].lectura = true;
      nuevosPermisos[4].escritura = true;
      nuevosPermisos[4].lectura = true;
      nuevosPermisos[5].escritura = false;
      nuevosPermisos[5].lectura = false;
      nuevosPermisos[6].escritura = false;
      nuevosPermisos[6].lectura = false;
      nuevosPermisos[7].escritura = false;
      nuevosPermisos[7].lectura = false;
      nuevosPermisos[8].escritura = false;
      nuevosPermisos[8].lectura = false;
      setPermisos(nuevosPermisos);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectUsuario]);

  useEffect(() => {
    if (!selectGrupo) {
      return;
    }
    if (selectGrupo === "1") {
      const nuevosPermisos = { ...permisosGrupo };
      nuevosPermisos[5].escritura = true;
      nuevosPermisos[5].lectura = true;
      nuevosPermisos[6].escritura = true;
      nuevosPermisos[6].lectura = true;
      nuevosPermisos[7].escritura = true;
      nuevosPermisos[7].lectura = true;
      nuevosPermisos[8].escritura = true;
      nuevosPermisos[8].lectura = true;
      setPermisos(nuevosPermisos);
    } else if (selectGrupo === "2") {
      const nuevosPermisos = { ...permisosGrupo };
      nuevosPermisos[2].lectura = true;
      nuevosPermisos[7].escritura = true;
      nuevosPermisos[7].lectura = true;
      nuevosPermisos[8].escritura = true;
      nuevosPermisos[8].lectura = true;
      setPermisos(nuevosPermisos);
    } else if (selectGrupo === "3") {
      const nuevosPermisos = { ...permisosGrupo };
      nuevosPermisos[0].escritura = true;
      nuevosPermisos[0].lectura = true;
      nuevosPermisos[1].escritura = true;
      nuevosPermisos[1].lectura = true;
      nuevosPermisos[2].escritura = true;
      nuevosPermisos[2].lectura = true;
      nuevosPermisos[3].escritura = true;
      nuevosPermisos[3].lectura = true;
      nuevosPermisos[4].escritura = true;
      nuevosPermisos[4].lectura = true;
      setPermisos(nuevosPermisos);
    } else if (selectGrupo === "4") {
      const nuevosPermisos = { ...permisosGrupo };
      nuevosPermisos[5].escritura = true;
      nuevosPermisos[5].lectura = true;
      setPermisos(nuevosPermisos);
    } else if (selectGrupo === "5") {
      const nuevosPermisos = { ...permisosGrupo };
      nuevosPermisos[7].escritura = true;
      nuevosPermisos[7].lectura = true;
      setPermisos(nuevosPermisos);
    } else if (selectGrupo === "6") {
      const nuevosPermisos = { ...permisosGrupo };
      nuevosPermisos[8].escritura = true;
      nuevosPermisos[8].lectura = true;
      setPermisos(nuevosPermisos);
    } else if (selectGrupo === "7") {
      const nuevosPermisos = { ...permisosGrupo };
      nuevosPermisos[8].escritura = true;
      nuevosPermisos[8].lectura = true;
      setPermisos(nuevosPermisos);
    } else if (selectGrupo === "8") {
      const nuevosPermisos = { ...permisosGrupo };
      nuevosPermisos[6].escritura = true;
      nuevosPermisos[6].lectura = true;
      setPermisos(nuevosPermisos);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectGrupo]);

  const establecerPermisosGrupo = (
    gruposSeleccionados: readonly SelectOpcion[]
  ) => {
    var nuevoGrupo = gruposSeleccionados.find(function (x) {
      // return elements in previousArray matching...
      return !selectGruposUsuarioAnterior.includes(x); // "this element doesn't exist in currentArray"
    });

    if (nuevoGrupo) {
      if (nuevoGrupo.value === "1") {
        const nuevosPermisos = { ...permisos };
        for (let index = 0; index < 5; index++) {
          nuevosPermisos[index].escritura = false;
          nuevosPermisos[index].lectura = false;
        }
        nuevosPermisos[5].escritura = true;
        nuevosPermisos[5].lectura = true;
        nuevosPermisos[6].escritura = true;
        nuevosPermisos[6].lectura = true;
        nuevosPermisos[7].escritura = true;
        nuevosPermisos[7].lectura = true;
        nuevosPermisos[8].escritura = true;
        nuevosPermisos[8].lectura = true;
        setPermisos(nuevosPermisos);
      } else if (nuevoGrupo.value === "2") {
        const nuevosPermisos = { ...permisos };
        nuevosPermisos[2].lectura = true;
        for (let index = 0; index < 9; index++) {
          nuevosPermisos[index].escritura = false;
          nuevosPermisos[index].lectura = false;
        }
        nuevosPermisos[7].escritura = true;
        nuevosPermisos[7].lectura = true;
        nuevosPermisos[8].escritura = true;
        nuevosPermisos[8].lectura = true;
        setPermisos(nuevosPermisos);
      } else if (nuevoGrupo.value === "3") {
        const nuevosPermisos = { ...permisos };
        for (let index = 0; index < 9; index++) {
          nuevosPermisos[index].escritura = false;
          nuevosPermisos[index].lectura = false;
        }
        nuevosPermisos[0].escritura = true;
        nuevosPermisos[0].lectura = true;
        nuevosPermisos[1].escritura = true;
        nuevosPermisos[1].lectura = true;
        nuevosPermisos[2].escritura = true;
        nuevosPermisos[2].lectura = true;
        nuevosPermisos[3].escritura = true;
        nuevosPermisos[3].lectura = true;
        nuevosPermisos[4].escritura = true;
        nuevosPermisos[4].lectura = true;
        setPermisos(nuevosPermisos);
      } else if (nuevoGrupo.value === "4") {
        const nuevosPermisos = { ...permisos };
        for (let index = 0; index < 9; index++) {
          nuevosPermisos[index].escritura = false;
          nuevosPermisos[index].lectura = false;
        }
        nuevosPermisos[5].escritura = true;
        nuevosPermisos[5].lectura = true;
        setPermisos(nuevosPermisos);
      } else if (nuevoGrupo.value === "5") {
        const nuevosPermisos = { ...permisos };
        for (let index = 0; index < 9; index++) {
          nuevosPermisos[index].escritura = false;
          nuevosPermisos[index].lectura = false;
        }
        nuevosPermisos[7].escritura = true;
        nuevosPermisos[7].lectura = true;
        setPermisos(nuevosPermisos);
      } else if (nuevoGrupo.value === "6") {
        const nuevosPermisos = { ...permisos };
        for (let index = 0; index < 9; index++) {
          nuevosPermisos[index].escritura = false;
          nuevosPermisos[index].lectura = false;
        }
        nuevosPermisos[8].escritura = true;
        nuevosPermisos[8].lectura = true;
        setPermisos(nuevosPermisos);
      } else if (nuevoGrupo.value === "7") {
        const nuevosPermisos = { ...permisos };
        for (let index = 0; index < 9; index++) {
          nuevosPermisos[index].escritura = false;
          nuevosPermisos[index].lectura = false;
        }
        nuevosPermisos[8].escritura = true;
        nuevosPermisos[8].lectura = true;
        setPermisos(nuevosPermisos);
      } else if (nuevoGrupo.value === "8") {
        const nuevosPermisos = { ...permisos };
        for (let index = 0; index < 9; index++) {
          nuevosPermisos[index].escritura = false;
          nuevosPermisos[index].lectura = false;
        }
        nuevosPermisos[6].escritura = true;
        nuevosPermisos[6].lectura = true;
        setPermisos(nuevosPermisos);
      }
    }
  };

  const [formularios, setFormularios] = useState<string[]>([
    "Mi CV",
    // "Mis postulaciones",
    "Detalle empleo",
    "Mi cuenta",
    "Reporte empleos",
    "Postulación empleo",
    "Respaldos",
    "Auditorias aplicación",
    "Administrar postulaciones",
    "Administrar empleos",
  ]);

  const [permisos, setPermisos] = useState<
    {
      formulario: string;
      lectura: boolean;
      idPermisoLectura: number;
      escritura: boolean;
      idPermisoEscritura: number;
    }[]
  >([
    {
      formulario: "Mi CV",
      lectura: false,
      idPermisoLectura: 1,
      escritura: false,
      idPermisoEscritura: 3,
    },
    // {
    //   formulario: "Mis postulaciones",
    //   lectura: false,
    //   idPermisoLectura: 4,
    //   escritura: false,
    //   idPermisoEscritura: 5,
    // },
    {
      formulario: "Detalle empleo",
      lectura: false,
      idPermisoLectura: 6,
      escritura: false,
      idPermisoEscritura: 7,
    },
    {
      formulario: "Mi cuenta",
      lectura: false,
      idPermisoLectura: 8,
      escritura: false,
      idPermisoEscritura: 9,
    },
    {
      formulario: "Reporte empleos",
      lectura: false,
      idPermisoLectura: 10,
      escritura: false,
      idPermisoEscritura: 0,
    },
    {
      formulario: "Postulación empleo",
      lectura: false,
      idPermisoLectura: 13,
      escritura: false,
      idPermisoEscritura: 14,
    },
    {
      formulario: "Respaldos",
      lectura: false,
      idPermisoLectura: 15,
      escritura: false,
      idPermisoEscritura: 16,
    },
    {
      formulario: "Auditorias aplicación",
      lectura: false,
      idPermisoLectura: 17,
      escritura: false,
      idPermisoEscritura: 0,
    },
    {
      formulario: "Administrar postulaciones",
      lectura: false,
      idPermisoLectura: 19,
      escritura: false,
      idPermisoEscritura: 20,
    },
    {
      formulario: "Administrar empleos",
      lectura: false,
      idPermisoLectura: 21,
      escritura: false,
      idPermisoEscritura: 22,
    },
  ]);

  const [permisosGrupo, setPermisosGrupo] = useState<
    {
      formulario: string;
      lectura: boolean;
      idPermisoLectura: number;
      escritura: boolean;
      idPermisoEscritura: number;
    }[]
  >([
    {
      formulario: "Mi CV",
      lectura: false,
      idPermisoLectura: 1,
      escritura: false,
      idPermisoEscritura: 3,
    },
    // {
    //   formulario: "Mis postulaciones",
    //   lectura: false,
    //   idPermisoLectura: 4,
    //   escritura: false,
    //   idPermisoEscritura: 5,
    // },
    {
      formulario: "Detalle empleo",
      lectura: false,
      idPermisoLectura: 6,
      escritura: false,
      idPermisoEscritura: 7,
    },
    {
      formulario: "Mi cuenta",
      lectura: false,
      idPermisoLectura: 8,
      escritura: false,
      idPermisoEscritura: 9,
    },
    {
      formulario: "Reporte empleos",
      lectura: false,
      idPermisoLectura: 10,
      escritura: false,
      idPermisoEscritura: 0,
    },
    {
      formulario: "Postulación empleo",
      lectura: false,
      idPermisoLectura: 13,
      escritura: false,
      idPermisoEscritura: 14,
    },
    {
      formulario: "Respaldos",
      lectura: false,
      idPermisoLectura: 15,
      escritura: false,
      idPermisoEscritura: 16,
    },
    {
      formulario: "Auditorias aplicación",
      lectura: false,
      idPermisoLectura: 17,
      escritura: false,
      idPermisoEscritura: 0,
    },
    {
      formulario: "Administrar postulaciones",
      lectura: false,
      idPermisoLectura: 19,
      escritura: false,
      idPermisoEscritura: 20,
    },
    {
      formulario: "Administrar empleos",
      lectura: false,
      idPermisoLectura: 21,
      escritura: false,
      idPermisoEscritura: 22,
    },
  ]);

  const [gruposPermisos, setGruposPermisos] = useState<string[]>([
    "Mi CV",
    "Mis postulaciones",
    "Detalle empleo",
    "Mi cuenta",
    "Reporte empleos",
    "Postulación empleo",
    "Respaldos",
    "Auditorias aplicación",
    "Administrar postulaciones",
    "Administrar empleos",
  ]);

  const { push } = useRouter();
  const [mensajeModal, setMensajeModal] = useState<string>();
  const [tituloModal, setTituloModal] = useState<string>("");

  useEffect(() => {
    // obtenerUsuarios();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const obtenerPermisos = async () => {
    await fetchPrivado("http://localhost:4000/empleos/usuarios", "GET")
      .then(async (data) => {
        if (data.ok) {
          const respuesta = await data.json();
          const empleos = respuesta.empleos as Array<{
            id: string;
            titulo: string;
            fechaPublicacion: string;
            ubicacion: string;
            empresa: string;
          }>;
          const empleosMap = empleos.map<{
            id: string;
            titulo: string;
            fechaPublicacion: string;
            ubicacion: string;
            empresa: string;
          }>(({ id, titulo, fechaPublicacion, ubicacion, empresa }) => ({
            id,
            titulo,
            fechaPublicacion,
            ubicacion,
            empresa,
          }));
          //setEmpleos(empleosMap);
          return;
        } else if (data.status === 400) {
          const respuesta = await data.json();
          const validaciones = respuesta.Validaciones.map(
            (validacion: { Mensaje: string }) => validacion.Mensaje
          ).join("\r\n");

          setMensajeModal(validaciones);

          return;
        } else if (data.status === 401) {
          push("/");
          return;
        }

        setMensajeModal(mensajeErrorGeneral);
      })
      .catch(() => {
        setMensajeModal(mensajeErrorGeneral);
      });
  };

  const limpiarModal = () => setMensajeModal(undefined);

  const backButtonClick = async () => {
    push("/admin");
  };

  const guardarClick = async () => {
    setTimeout(() => {
      setTituloModal("Permisos");
      setMensajeModal("Los permisos han sido guardados");
    }, 3000);
  };

  return (
    <Layout userLayout={false}>
      <div className="cuerpo">
        <Tabs
          selectedIndex={tabSeleccionado}
          onSelect={(indice) => setTabSeleccionado(indice)}
        >
          <TabList className="flex font-bold">
            <Tab
              className={`border p-3 ${
                tabSeleccionado === 0 ? "tab-seleccionado" : ""
              }`}
            >
              Usuarios
            </Tab>
            <Tab
              className={`border p-3 ${
                tabSeleccionado === 1 ? "tab-seleccionado" : ""
              }`}
            >
              Grupos
            </Tab>
          </TabList>
          <TabPanel className="border p-3">
            <div>
              <label className="block font-bold">Usuario</label>
              <Select
                className="mt-2"
                placeholder=""
                isLoading={!selectUsuarios}
                isMulti={false}
                onChange={(opcionSeleccionada: SingleValue<SelectOpcion>) => {
                  opcionSeleccionada &&
                    setSelectUsuario(opcionSeleccionada.value);
                }}
                options={selectUsuarios}
                value={selectUsuarios.filter(function (opcion) {
                  return opcion.value === selectUsuario;
                })}
                isClearable={false}
              />
            </div>
            {selectUsuario && (
              <div className="mt-2">
                <label className="block font-bold">Grupos</label>
                <Select
                  id="gruposUsuario"
                  className="mt-2"
                  placeholder=""
                  isLoading={!selectGrupos}
                  isMulti={true}
                  onChange={(opcionSeleccionada: readonly SelectOpcion[]) => {
                    setSelectGruposUsuario(opcionSeleccionada);
                    establecerPermisosGrupo(opcionSeleccionada);
                  }}
                  options={selectGrupos}
                  value={selectGruposUsuario}
                />
              </div>
            )}
            {selectUsuario && (
              <div className="mt-2">
                <label className="block font-bold">Permisos</label>
                {formularios.map((nombre, i) => (
                  <div className="flex mt-2" key={i}>
                    <label className="admin-permisos-formulario-item">
                      {nombre}
                    </label>
                    {permisos[i].idPermisoLectura !== 0 && (
                      <label className="ml-3 flex">
                        <input
                          type="checkbox"
                          checked={permisos[i].lectura}
                          className="mr-1"
                          value="Lectura"
                          onChange={(e) => {
                            const nuevosPermisos = { ...permisos };
                            nuevosPermisos[i].lectura = e.target.checked;
                            setPermisos(nuevosPermisos);
                          }}
                        />
                        Lectura
                      </label>
                    )}
                    {permisos[i].idPermisoEscritura !== 0 && (
                      <label className="flex ml-3">
                        <input
                          type="checkbox"
                          checked={permisos[i].escritura}
                          className="mr-1"
                          value="Escritura"
                          onChange={(e) => {
                            const nuevosPermisos = { ...permisos };
                            nuevosPermisos[i].escritura = e.target.checked;
                            setPermisos(nuevosPermisos);
                          }}
                        />
                        Escritura
                      </label>
                    )}
                  </div>
                ))}
              </div>
            )}
          </TabPanel>
          <TabPanel>
            <div>
              <label className="block font-bold">Grupo</label>
              <Select
                className="mt-2"
                placeholder=""
                isLoading={!selectGrupos}
                isMulti={false}
                onChange={(opcionSeleccionada: SingleValue<SelectOpcion>) => {
                  opcionSeleccionada &&
                    setSelectGrupo(opcionSeleccionada.value);
                }}
                options={selectGrupos}
                value={selectGrupos.filter(function (opcion) {
                  return opcion.value === selectGrupo;
                })}
                isClearable={false}
              />
            </div>
            {selectGrupo && (
              <div className="mt-2">
                <label className="block font-bold">Permisos</label>
                {formularios.map((nombre, i) => (
                  <div className="flex mt-2" key={i}>
                    <label className="admin-permisos-formulario-item">
                      {nombre}
                    </label>
                    {permisosGrupo[i].idPermisoLectura !== 0 && (
                      <label className="ml-3 flex">
                        <input
                          type="checkbox"
                          disabled
                          checked={permisosGrupo[i].lectura}
                          className="mr-1"
                          value="Lectura"
                          onChange={(e) => {
                            const nuevosPermisos = { ...permisosGrupo };
                            nuevosPermisos[i].lectura = e.target.checked;
                            setPermisos(nuevosPermisos);
                          }}
                        />
                        Lectura
                      </label>
                    )}
                    {permisosGrupo[i].idPermisoEscritura !== 0 && (
                      <label className="flex ml-3">
                        <input
                          type="checkbox"
                          disabled
                          checked={permisosGrupo[i].escritura}
                          className="mr-1"
                          value="Escritura"
                          onChange={(e) => {
                            const nuevosPermisos = { ...permisosGrupo };
                            nuevosPermisos[i].escritura = e.target.checked;
                            setPermisos(nuevosPermisos);
                          }}
                        />
                        Escritura
                      </label>
                    )}
                  </div>
                ))}
              </div>
            )}
          </TabPanel>
        </Tabs>
        <div className="flex mt-2">
          <button onClick={backButtonClick} type="button" className="boton">
            Volver
          </button>

          <button
            onClick={guardarClick}
            type="button"
            className="boton ml-2 btn-primary"
          >
            Guardar
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
