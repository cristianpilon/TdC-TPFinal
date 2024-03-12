import { FC, MouseEventHandler, useEffect, useState } from "react";
import Select, { SingleValue } from "react-select";
import Modal from "../compartido/modal";
import { SelectOpcion } from "../compartido/select/selectOpcion";
import {
  idiomasSistema,
  idiomasSistemaNiveles,
} from "../compartido/idiomas/idiomas-sistema";

interface ModalProps {
  mostrar: boolean;
  idiomasCv: string[];
  onCambiarModal: MouseEventHandler<HTMLButtonElement>;
  onAceptarAccion: Function;
}

const ModalIdioma: FC<ModalProps> = ({
  mostrar,
  idiomasCv,
  onCambiarModal,
  onAceptarAccion,
}) => {
  const [nivel, setNivel] = useState<string>();
  const [niveles, setNiveles] = useState<SelectOpcion[]>([]);
  const [idioma, setIdioma] = useState<string>();
  const [idiomas, setIdiomas] = useState<SelectOpcion[]>([]);

  useEffect(() => {
    setNiveles(idiomasSistemaNiveles);
    setIdiomas(idiomasSistema.filter((x) => !idiomasCv.includes(x.value)));
  }, [idiomasCv]);

  const clickAceptar = () => {
    onAceptarAccion(idioma, nivel);
  };

  return (
    <Modal
      mostrar={mostrar}
      titulo="Idioma"
      onCambiarModal={onCambiarModal}
      onAceptarAccion={clickAceptar}
    >
      <form className="text-left m-2" style={{ minWidth: "500px" }}>
        <div>
          <label className="block font-bold">Agregar idioma</label>
          <Select
            className="mt-2"
            placeholder=""
            isLoading={!idiomas}
            isMulti={false}
            onChange={(opcionSeleccionada: SingleValue<SelectOpcion>) => {
              opcionSeleccionada && setIdioma(opcionSeleccionada.value);
            }}
            options={idiomas}
            value={idiomas.filter(function (opcion) {
              return opcion.value === idioma;
            })}
            isClearable={false}
          />
        </div>
        <div className="mt-2">
          <label className="block font-bold">Nivel</label>
          <Select
            className="mt-2"
            placeholder=""
            isLoading={!niveles}
            isMulti={false}
            onChange={(opcionSeleccionada: SingleValue<SelectOpcion>) => {
              opcionSeleccionada && setNivel(opcionSeleccionada.value);
            }}
            options={niveles}
            value={niveles.filter(function (opcion) {
              return opcion.value === nivel;
            })}
            isClearable={false}
          />
        </div>
      </form>
    </Modal>
  );
};

export default ModalIdioma;
