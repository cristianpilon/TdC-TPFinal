import { FC } from "react";

interface FormularioProps {
  empleo: Array<{
    id: string;
    titulo: string;
    fechaPublicacion: string;
    ubicacion: string;
    empresa: string;
    empresaLogo: string;
    destacado: boolean;
    nuevo: boolean;
    perfiles: Array<string>;
    etiquetas: Array<string>;
  }>;
}

const FormularioEmpleo: FC<FormularioProps> = ({ empleo }) => {
  console.log(empleo);
  return <></>;
};

export default FormularioEmpleo;
