import { FC, MouseEventHandler, useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import Modal from "../compartido/modal";

interface ModalProps {
  mostrar: boolean;
  estudio: {
    idx: number;
    institucion: string;
    nivel: string;
    ingreso: Date;
    egreso: Date | null;
    titulo: string;
  };
  onCambiarModal: MouseEventHandler<HTMLButtonElement>;
  onAceptarAccion: Function;
}

const ModalEstudio: FC<ModalProps> = ({
  mostrar,
  estudio,
  onCambiarModal,
  onAceptarAccion,
}) => {
  const [institucion, setInstitucion] = useState(estudio.institucion);
  const [nivel, setNivel] = useState(estudio.nivel);
  const [ingreso, setIngreso] = useState(estudio.ingreso ?? new Date());
  const [egreso, setEgreso] = useState(estudio.egreso);
  const [titulo, setTitulo] = useState(estudio.titulo);

  const clickAceptar = () => {
    const nuevoEstudio = {
      institucion,
      nivel,
      ingreso,
      egreso,
      titulo,
    };
    onAceptarAccion(nuevoEstudio);
  };

  return (
    <Modal
      mostrar={mostrar}
      titulo="Estudio"
      onCambiarModal={onCambiarModal}
      onAceptarAccion={clickAceptar}
    >
      <form className="text-left m-2" style={{ minWidth: "500px" }}>
        <div>
          <label className="block font-bold">Institución</label>
          <input
            type="text"
            id="institucion"
            name="institucion"
            value={institucion}
            className="border w-full mt-1"
            onChange={(e) => setInstitucion(e.target.value)}
            required
          />
        </div>
        <div className="mt-2">
          <label className="block font-bold">Nivel</label>
          <input
            type="text"
            id="cargo"
            name="cargo"
            value={nivel}
            className="border w-full mt-1"
            onChange={(e) => setNivel(e.target.value)}
            required
          />
        </div>
        <div className="mt-2">
          <label className="block font-bold">Ingreso</label>
          <DatePicker
            className="border"
            selected={ingreso}
            onChange={(fecha: Date) => setIngreso(fecha)}
            dateFormat="dd/MM/yyyy"
          />
        </div>
        <div className="mt-2">
          <label className="block font-bold">Egreso</label>
          <DatePicker
            className="border"
            selected={egreso}
            onChange={(fecha: Date) => setEgreso(fecha)}
            dateFormat="dd/MM/yyyy"
          />
        </div>
        <div className="mt-2">
          <label className="block font-bold">Título</label>
          <input
            type="text"
            id="titulo"
            name="titulo"
            value={titulo}
            className="border w-full mt-1"
            onChange={(e) => setTitulo(e.target.value)}
            required
          />
        </div>
      </form>
    </Modal>
  );
};

export default ModalEstudio;
