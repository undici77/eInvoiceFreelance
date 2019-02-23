#!/bin/sh

cp ../../eInvoiceFreelance/bin/Release/eInvoiceFreelance.exe eInvoiceFreelance/opt/eInvoiceFreelance/eInvoiceFreelance.exe
cp ../../eInvoiceFreelance/bin/Release/itextsharp.dll eInvoiceFreelance/opt/eInvoiceFreelance/itextsharp.dll
dpkg-deb --build eInvoiceFreelance/


