import {
  FC,
  MouseEventHandler,
  useState,
  useEffect,
  Dispatch,
  SetStateAction,
} from "react";
import { Bar } from "react-chartjs-2";
import { CategoryScale } from "chart.js";
import Chart from "chart.js/auto";

import Modal from "../compartido/modal";
import Spinner from "../compartido/spinner";

Chart.register(CategoryScale);

const colores = [
  "rgba(255, 102, 102, 1)",
  "rgba(102, 179, 255, 1)",
  "rgba(124, 204, 124, 1)",
  "#F8F9FA",
  "#FFD666",
  "#70ADDE",
  "rgba(255, 140, 66, 1)",
  "#FF6666",
  "#96D4FF",
  "#E9ECEF",
  "#7CFC00",
  "#DEE2E6",
  "#F472D0",
  "#F54D4D",
  "#CED4DA",
  "#64B800",
  "#A9A9A9",
  "#333333",
  "#4D86C6",
  "#F9AA33",
];

interface ModalProps {
  mostrar: boolean;
  empleos: Array<{
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
  onCambiarModal: Dispatch<SetStateAction<boolean>>;
}

const ModalReporte: FC<ModalProps> = ({ mostrar, empleos, onCambiarModal }) => {
  const [datosPerfiles, setDatosPerfiles] = useState<{
    labels: any[];
    datasets: {
      label: string;
      data: any[];
      backgroundColor: string[];
      borderColor: string;
      borderWidth: number;
    }[];
  }>();

  const [datosEtiquetas, setDatosEtiquetas] = useState<{
    labels: any[];
    datasets: {
      label: string;
      data: any[];
      backgroundColor: string[];
      borderColor: string;
      borderWidth: number;
    }[];
  }>();

  useEffect(() => {
    try {
      prepararDatosPerfiles();
      prepararDatosEtiquetas();
    } catch {}
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const prepararDatosPerfiles = () => {
    let perfiles: any = {};
    for (const empleo of empleos) {
      for (const perfil of empleo.perfiles) {
        if (perfiles.hasOwnProperty(perfil)) {
          perfiles[perfil]++;
          continue;
        }
        perfiles[perfil] = 1;
      }
    }

    const valores = {
      labels: Object.keys(perfiles).map((p: any) => p),
      datasets: [
        {
          label: "Empleos",
          data: Object.keys(perfiles).map((p: any) => perfiles[p]),
          backgroundColor: colores,
          borderColor: "black",
          borderWidth: 2,
        },
      ],
    };
    setDatosPerfiles(valores);
  };

  const prepararDatosEtiquetas = () => {
    let etiquetas: any = {};
    for (const empleo of empleos) {
      for (const perfil of empleo.etiquetas) {
        if (etiquetas.hasOwnProperty(perfil)) {
          etiquetas[perfil]++;
          continue;
        }
        etiquetas[perfil] = 1;
      }
    }

    const valores = {
      labels: Object.keys(etiquetas).map((p: any) => p),
      datasets: [
        {
          label: "Empleos",
          data: Object.keys(etiquetas).map((p: any) => etiquetas[p]),
          backgroundColor: colores,
          borderColor: "black",
          borderWidth: 2,
        },
      ],
    };
    setDatosEtiquetas(valores);
  };

  const cerrarModalClick: MouseEventHandler<HTMLButtonElement> = () => {
    onCambiarModal(false);
  };

  return (
    <div className="contenedor-reporte">
      <Modal
        mostrar={mostrar}
        titulo="Reporte de empleos"
        onCambiarModal={cerrarModalClick}
      >
        <h2 style={{ textAlign: "center" }}>Reporte de empleos</h2>
        <>
          {datosPerfiles && datosEtiquetas && (
            <>
              <div>
                <Bar
                  data={datosPerfiles}
                  options={{
                    maintainAspectRatio: false,
                    plugins: {
                      title: {
                        display: true,
                        text: "Perfiles",
                      },
                      legend: {
                        display: false,
                      },
                    },
                    scales: {
                      y: {
                        suggestedMin: 0,
                        suggestedMax: empleos.length,
                        ticks: {
                          precision: 0,
                        },
                      },
                    },
                  }}
                />
              </div>
              <div>
                <Bar
                  data={datosEtiquetas}
                  options={{
                    maintainAspectRatio: false,
                    plugins: {
                      title: {
                        display: true,
                        text: "Etiquetas",
                      },
                      legend: {
                        display: false,
                      },
                    },
                    scales: {
                      y: {
                        suggestedMin: 0,
                        suggestedMax: empleos.length,
                        ticks: {
                          precision: 0,
                        },
                      },
                    },
                  }}
                />
              </div>
            </>
          )}
          {(!datosPerfiles || !datosEtiquetas) && <Spinner />}
        </>
      </Modal>
    </div>
  );
};

export default ModalReporte;
