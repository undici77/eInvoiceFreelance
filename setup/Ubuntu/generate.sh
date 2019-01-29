#!/bin/sh

cp ../../eInvoiceFreelance/bin/Release/eInvoiceFreelance.exe eInvoiceFreelance/opt/eInvoiceFreelance/eInvoiceFreelance.exe
dpkg-deb --build eInvoiceFreelance/


