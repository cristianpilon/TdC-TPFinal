import { FC, MouseEventHandler, useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import Modal from "../compartido/modal";

interface ModalProps {
  mostrar: boolean;
  empleo: {
    idx: number;
    organizacion: string;
    cargo: string;
    ingreso: Date;
    egreso: Date | null;
    area: string;
    funciones: string;
  };
  onCambiarModal: MouseEventHandler<HTMLButtonElement>;
  onAceptarAccion: Function;
}

const ModalEmpleo: FC<ModalProps> = ({
  mostrar,
  empleo,
  onCambiarModal,
  onAceptarAccion,
}) => {
  const [organizacion, setOrganizacion] = useState(empleo.organizacion);
  const [cargo, setCargo] = useState(empleo.cargo);
  const [ingreso, setIngreso] = useState(empleo.ingreso ?? new Date());
  const [egreso, setEgreso] = useState(empleo.egreso);
  const [area, setArea] = useState(empleo.area);
  const [funciones, setFunciones] = useState(empleo.funciones);

  const clickAceptar = () => {
    const nuevoEmpleo = {
      organizacion,
      cargo,
      ingreso,
      egreso,
      area,
      funciones,
    };
    onAceptarAccion(nuevoEmpleo);
  };

  return (
    <Modal
      mostrar={mostrar}
      titulo="Empleo"
      onCambiarModal={onCambiarModal}
      onAceptarAccion={clickAceptar}
    >
      <form className="text-left m-2" style={{ minWidth: "500px" }}>
        <div>
          <label className="block font-bold">Organización</label>
          <input
            type="text"
            id="organizacion"
            name="organizacion"
            value={organizacion}
            className="border w-full mt-1"
            onChange={(e) => setOrganizacion(e.target.value)}
            required
          />
        </div>
        <div className="mt-2">
          <label className="block font-bold">Cargo</label>
          <input
            type="text"
            id="cargo"
            name="cargo"
            value={cargo}
            className="border w-full mt-1"
            onChange={(e) => setCargo(e.target.value)}
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
          <label className="block font-bold">Area</label>
          <input
            type="text"
            id="area"
            name="area"
            value={area}
            className="border w-full mt-1"
            onChange={(e) => setArea(e.target.value)}
            required
          />
        </div>
        <div className="mt-2">
          <label className="block font-bold">Funciones</label>
          <textarea
            cols={30}
            rows={3}
            id="funciones"
            name="funciones"
            value={funciones}
            className="border w-full mt-1"
            onChange={(e) => setFunciones(e.target.value)}
            required
          />
        </div>
      </form>
    </Modal>
  );
};

export default ModalEmpleo;
