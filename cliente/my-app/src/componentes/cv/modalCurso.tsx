import { FC, MouseEventHandler, useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import Modal from "../compartido/modal";

interface ModalProps {
  mostrar: boolean;
  curso: {
    idx: number;
    institucion: string;
    titulo: string;
    fecha: Date;
  };
  onCambiarModal: MouseEventHandler<HTMLButtonElement>;
  onAceptarAccion: Function;
}

const ModalCurso: FC<ModalProps> = ({
  mostrar,
  curso,
  onCambiarModal,
  onAceptarAccion,
}) => {
  const [institucion, setInstitucion] = useState(curso.institucion);
  const [titulo, setTitulo] = useState(curso.titulo);
  const [fecha, setFecha] = useState(curso.fecha ?? new Date());

  const clickAceptar = () => {
    const nuevoCurso = {
      institucion,
      fecha,
      titulo,
    };
    onAceptarAccion(nuevoCurso);
  };

  return (
    <Modal
      mostrar={mostrar}
      titulo="Curso"
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
        <div className="mt-2">
          <label className="block font-bold">Fecha</label>
          <DatePicker
            className="border"
            selected={fecha}
            onChange={(fecha: Date) => setFecha(fecha)}
            dateFormat="dd/MM/yyyy"
          />
        </div>
      </form>
    </Modal>
  );
};

export default ModalCurso;
