import React, { MouseEventHandler, FC } from "react";

interface ModalProps {
  mostrar: boolean;
  titulo: string;
  onCambiarModal: MouseEventHandler<HTMLButtonElement>;
  children: React.ReactNode;
  onAceptarAccion?: MouseEventHandler<HTMLButtonElement>;
}

const Modal: FC<ModalProps> = ({
  mostrar,
  titulo,
  onCambiarModal,
  children,
  onAceptarAccion,
}) => {
  return (
    <>
      <div
        className={`modal-overlay ${
          mostrar ? "block" : "invisible md:visible md:flex"
        }`}
        aria-labelledby="modal-title"
        role="dialog"
        aria-modal="true"
      >
        <div className="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
          <div
            className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity"
            aria-hidden="true"
          ></div>

          <span
            className="hidden sm:inline-block sm:align-middle sm:h-screen"
            aria-hidden="true"
          >
            &#8203;
          </span>

          <div className="modal-size uai-shadow border relative inline-block align-bottom bg-white rounded-lg overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
            <div className="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
              <div className="sm:flex sm:items-start">
                <div className="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left">
                  <h3
                    className="text-left py-2 text-lg leading-6 font-medium text-gray-900 border-b"
                    id="modal-title"
                  >
                    <strong>{titulo}</strong>
                  </h3>
                  <div className="mt-2">{children}</div>
                </div>
              </div>
            </div>
            <div className="text-right bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
              <button type="button" className="boton" onClick={onCambiarModal}>
                {onAceptarAccion ? "Cancelar" : "Cerrar"}
              </button>
              {onAceptarAccion && (
                <button
                  type="button"
                  className="boton ml-2"
                  onClick={onAceptarAccion}
                >
                  Aceptar
                </button>
              )}
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default Modal;
