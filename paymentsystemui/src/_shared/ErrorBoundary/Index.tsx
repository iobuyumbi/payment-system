// Base Imports
import * as React from 'react'

interface IErrorBoundaryState {
  readonly hasError: boolean;
  readonly error: any; 
  readonly info: any;
  readonly stack: any;
  readonly children?: React.ReactNode;
}

class ErrorBoundary extends React.Component<React.PropsWithChildren<{}>, IErrorBoundaryState> {
  constructor(props: any) {
    super(props);
    this.state = { hasError: false, error: '', info: '', stack: '' , children: <></>}
  }

  static getDerivedStateFromError(error: any) {
    // Update state so the next render will show the fallback UI.
    return { hasError: true };
  }

  componentDidCatch(error: any, errorInfo: any) {
    // You can also log the error to an error reporting service
    // logErrorToMyService(error, errorInfo)
    this.setState({hasError: false, error, info: errorInfo, stack: error.stack, children: <></> })
  }

  render() {
    if (this.state.hasError) {
      // You can render any custom fallback UI
      return <h4>Something went wrong!</h4>;
    }

    return 
    <>
    {this.props.children}
    </>
  }
}

export default ErrorBoundary